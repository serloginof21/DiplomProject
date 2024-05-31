using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiplomProject
{
    public partial class AddStudent : Window
    {
        private Random random = new Random();
        public ChampionatEntities db = new ChampionatEntities();

        public AddStudent()
        {
            InitializeComponent();

            cb1.ItemsSource = db.Category.Where(c => c.Id_Category == 1).ToList(); ;
            cb2.ItemsSource = db.Organizations.ToList();
            cb3.ItemsSource = db.ClothingSizes.ToList();
            cb4.ItemsSource = db.Competences.ToList();
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            Students stWin = new Students();
            stWin.Show();
            this.Close();
        }

        private void GenerationId_ClickButton(object sender, RoutedEventArgs e)
        {
            string newID = GenerateRandomID();
            tbId.Text = newID;
        }

        private string GenerateRandomID()
        {
            const string chars = "0123456789";
            int randomID;
            string newID;
            do
            {
                newID = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                randomID = int.Parse(newID);
            } while (db.Student.Any(p => p.Id_Student == randomID));
            return newID;
        }

        private bool FieldsAreValid()
        {
            return !string.IsNullOrWhiteSpace(tb1.Text) &&
                   !string.IsNullOrWhiteSpace(tb2.Text) &&
                   !string.IsNullOrWhiteSpace(tb3.Text) &&
                   !string.IsNullOrWhiteSpace(tb4.Text) &&
                   !string.IsNullOrWhiteSpace(tb6.Text) &&
                   !string.IsNullOrWhiteSpace(tb7.Text) &&
                   cb1.SelectedItem != null &&
                   cb2.SelectedItem != null &&
                   cb3.SelectedItem != null;
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            if (!FieldsAreValid())
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!IsValidEmail(tb4.Text))
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!IsValidPhoneNumber(tb7.Text))
            {
                MessageBox.Show("Пожалуйста, введите корректный номер телефона.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Student s = new Student();
                s.Id_Student = Convert.ToInt32(tbId.Text);
                s.SurnameStudent = tb1.Text;
                s.NameStudent = tb2.Text;
                s.PatronymicStudent = tb3.Text;
                s.EmailStudent = tb4.Text;
                s.RegionStudent = tb5.Text;
                s.CountryStudent = tb6.Text;
                s.PhoneNumberStudent = tb7.Text;

                Category selectedCategory = cb1.SelectedItem as Category;
                if (selectedCategory != null)
                {
                    s.Id_Category = selectedCategory.Id_Category;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите категорию.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Organizations selectedOrganization = cb2.SelectedItem as Organizations;
                if (selectedOrganization != null)
                {
                    s.Id_Organization = selectedOrganization.Id_Organization;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите организацию.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                ClothingSizes selectedSize = cb3.SelectedItem as ClothingSizes;
                if (selectedSize != null)
                {
                    s.Id_ClothingSizeStudent = selectedSize.Id_ClothingSize;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите размер одежды.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Competences selectedCompetence = cb4.SelectedItem as Competences;
                if (selectedCompetence != null)
                {
                    s.Id_Competence = selectedCompetence.Id_Competence;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите компетенцию.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите добавить новую запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Student.Add(s);
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены в базу данных.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных в базу данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrganization = (Organizations)cb2.SelectedItem;

            if (selectedOrganization != null)
            {
                tb6.Text = selectedOrganization.Country;
                tb5.Text = selectedOrganization.Region;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
            {
                int selectionStart = textBox.SelectionStart;
                string newText = CapitalizeFirstLetter(textBox.Text);
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart;
            }
        }
        private string CapitalizeFirstLetter(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.ToLower();
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        private void tb7_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string input = textBox.Text;

                if (!input.StartsWith("8"))
                {
                    input = "8";
                }

                input = "8" + new string(input.Skip(1).Where(char.IsDigit).Take(10).ToArray());

                textBox.Text = input;
                textBox.SelectionStart = input.Length;
            }
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^8\d{10}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
