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
    public partial class EditStudents : Window
    {
        private Student selectedStudent;
        public ChampionatEntities db;

        public EditStudents(Student student, ChampionatEntities db)
        {
            InitializeComponent();
            selectedStudent = student;
            this.db = db;
            tbId.Text = selectedStudent.Id_Student.ToString();
            tb1.Text = selectedStudent.SurnameStudent;
            tb2.Text = selectedStudent.NameStudent;
            tb3.Text = selectedStudent.PatronymicStudent;
            tb4.Text = selectedStudent.EmailStudent;
            tb5.Text = selectedStudent.Organizations.Region;
            tb6.Text = selectedStudent.Organizations.Country;
            tb7.Text = selectedStudent.PhoneNumberStudent;

            cb1.ItemsSource = db.Category.Where(c => c.Id_Category == 1).ToList();
            cb1.SelectedItem = db.Category.FirstOrDefault(r => r.NameCategory == selectedStudent.Category.NameCategory);

            cb2.ItemsSource = db.Organizations.ToList();
            cb2.SelectedItem = db.Organizations.FirstOrDefault(o => o.Organization == selectedStudent.Organizations.Organization);

            cb2.SelectionChanged += cb2_SelectionChanged;
            cb3.ItemsSource = db.ClothingSizes.ToList();
            cb3.SelectedItem = db.ClothingSizes.FirstOrDefault(c => c.NameSize == selectedStudent.ClothingSizes.NameSize);

            cb4.ItemsSource = db.Competences.ToList();
            cb4.SelectedItem = db.Competences.FirstOrDefault(r => r.NameCompetence == selectedStudent.Competences.NameCompetence);
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            Students stWin = new Students();
            stWin.Show();
            this.Close();
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
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            if (!IsValidEmail(tb4.Text))
            {
                MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.");
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                selectedStudent.SurnameStudent = tb1.Text;
                selectedStudent.NameStudent = tb2.Text;
                selectedStudent.PatronymicStudent = tb3.Text;
                selectedStudent.EmailStudent = tb4.Text;
                selectedStudent.RegionStudent = tb5.Text;
                selectedStudent.CountryStudent = tb6.Text;
                selectedStudent.PhoneNumberStudent = tb7.Text;

                selectedStudent.Id_Category = ((Category)cb1.SelectedItem).Id_Category;
                selectedStudent.Id_Organization = ((Organizations)cb2.SelectedItem).Id_Organization;
                selectedStudent.Id_ClothingSizeStudent = ((ClothingSizes)cb3.SelectedItem).Id_ClothingSize;
                selectedStudent.Id_Competence = ((Competences)cb4.SelectedItem).Id_Competence;

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
