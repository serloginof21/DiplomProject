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

            cb1.ItemsSource = db.Category.ToList();
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
            } while (db.Student.Any(p => p.Id_Student == randomID)); // Проверка наличия ID в базе данных
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
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            if (!IsValidEmail(tb4.Text))
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.");
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
                    MessageBox.Show("Пожалуйста, выберите категорию.");
                    return;
                }

                Organizations selectedOrganization = cb2.SelectedItem as Organizations;
                if (selectedOrganization != null)
                {
                    s.Id_Organization = selectedOrganization.Id_Organization;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите организацию.");
                    return;
                }

                ClothingSizes selectedSize = cb3.SelectedItem as ClothingSizes;
                if (selectedSize != null)
                {
                    s.Id_ClothingSizeStudent = selectedSize.Id_ClothingSize;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите размер одежды.");
                    return;
                }

                Competences selectedCompetence = cb4.SelectedItem as Competences;
                if (selectedCompetence != null)
                {
                    s.Id_Competence = selectedCompetence.Id_Competence;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите компетенцию.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите добавить новую запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Student.Add(s);
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены в базу данных.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных в базу данных: {ex.Message}");
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
    }
}
