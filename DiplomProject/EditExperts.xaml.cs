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
    public partial class EditExperts : Window
    {
        private Expert selectedExpert;
        public ChampionatEntities db;

        public EditExperts(Expert expert, ChampionatEntities db)
        {
            InitializeComponent();
            selectedExpert = expert;
            this.db = db;
            tbId.Text = selectedExpert.Id_Expert.ToString();
            tb1.Text = selectedExpert.SurnameExpert;
            tb2.Text = selectedExpert.NameExpert;
            tb3.Text = selectedExpert.PatronymicExpert;
            tb4.Text = selectedExpert.EmailExpert;
            tb5.Text = selectedExpert.Organizations.Region;
            tb6.Text = selectedExpert.Organizations.Country;
            tb7.Text = selectedExpert.PhoneNumberExpert;

            cb1.ItemsSource = db.ExpertRole.ToList();
            cb1.SelectedItem = db.ExpertRole.FirstOrDefault(r => r.NameRole == selectedExpert.ExpertRole.NameRole);

            cb2.ItemsSource = db.Organizations.ToList();
            cb2.SelectedItem = db.Organizations.FirstOrDefault(o => o.Organization == selectedExpert.Organizations.Organization);

            cb2.SelectionChanged += cb2_SelectionChanged;
            cb3.ItemsSource = db.ClothingSizes.ToList();
            cb3.SelectedItem = db.ClothingSizes.FirstOrDefault(c => c.NameSize == selectedExpert.ClothingSizes.NameSize);

            cb4.ItemsSource = db.Competences.ToList();
            cb4.SelectedItem = db.Competences.FirstOrDefault(r => r.NameCompetence == selectedExpert.Competences.NameCompetence);
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
                   cb3.SelectedItem != null &&
                   cb4.SelectedItem != null;
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {
            if (!FieldsAreValid())
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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

            var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                selectedExpert.SurnameExpert = tb1.Text;
                selectedExpert.NameExpert = tb2.Text;
                selectedExpert.PatronymicExpert = tb3.Text;
                selectedExpert.EmailExpert = tb4.Text;
                selectedExpert.RegionExpert = tb5.Text;
                selectedExpert.CountryExpert = tb6.Text;
                selectedExpert.PhoneNumberExpert = tb7.Text;

                selectedExpert.Id_Role = ((ExpertRole)cb1.SelectedItem).Id_Role;
                selectedExpert.Id_Organization = ((Organizations)cb2.SelectedItem).Id_Organization;
                selectedExpert.Id_ClothingSizeExpert = ((ClothingSizes)cb3.SelectedItem).Id_ClothingSize;
                selectedExpert.Id_Competence = ((Competences)cb4.SelectedItem).Id_Competence;

                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Изменения сохранены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            WindowExperts exWin = new WindowExperts();
            exWin.Show();
            this.Close();
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
