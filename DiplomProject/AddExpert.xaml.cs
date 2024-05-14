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
    public partial class AddExpert : Window
    {
        private Random random = new Random();
        public ChampionatEntities db = new ChampionatEntities();

        public AddExpert()
        {
            InitializeComponent();

            cb1.ItemsSource = db.ExpertRole.ToList();
            cb2.ItemsSource = db.Organizations.ToList();
            cb3.ItemsSource = db.ClothingSizes.ToList();
            cb4.ItemsSource = db.Competences.ToList();
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
                Expert expert = new Expert();
                expert.Id_Expert = Convert.ToInt32(tbId.Text);
                expert.SurnameExpert = tb1.Text;
                expert.NameExpert = tb2.Text;
                expert.PatronymicExpert = tb3.Text;
                expert.EmailExpert = tb4.Text;
                expert.RegionExpert = tb5.Text;
                expert.CountryExpert = tb6.Text;
                expert.PhoneNumberExpert = tb7.Text;

                ExpertRole selectedCategory = cb1.SelectedItem as ExpertRole;
                if (selectedCategory != null)
                {
                    expert.Id_Role = selectedCategory.Id_Role;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите категорию.");
                    return;
                }

                Organizations selectedOrganization = cb2.SelectedItem as Organizations;
                if (selectedOrganization != null)
                {
                    expert.Id_Organization = selectedOrganization.Id_Organization;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите организацию.");
                    return;
                }

                ClothingSizes selectedSize = cb3.SelectedItem as ClothingSizes;
                if (selectedSize != null)
                {
                    expert.Id_ClothingSizeExpert = selectedSize.Id_ClothingSize;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите размер одежды.");
                    return;
                }

                Competences selectedCompetence = cb4.SelectedItem as Competences;
                if (selectedCompetence != null)
                {
                    expert.Id_Competence = selectedCompetence.Id_Competence;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите компетенцию.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите добавить новую запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Expert.Add(expert);
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены в базу данных.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных в базу данных: {ex.Message}");
            }
        }

        private void GenerationId_ClickButton(object sender, RoutedEventArgs e)
        {
            string newID = GenerateRandomID();
            tbId.Text = newID;
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
            } while (db.Expert.Any(p => p.Id_Expert == randomID)); // Проверка наличия ID в базе данных
            return newID;
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
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
    }
}
