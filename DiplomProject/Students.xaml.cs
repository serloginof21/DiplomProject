using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class Students : Window
    {
        public ChampionatEntities db;
        private List<Student> allStudent;

        public Students()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            allStudent = db.Student.ToList();
            dgS.ItemsSource = db.Student.ToList();
            tbSearch.TextChanged += tbSearch_TextChanged;
        }


        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            AddStudent addStWin = new AddStudent();
            addStWin.Show();
            this.Close();
        }

        private void Delete_ClickButton(object sender, RoutedEventArgs e)
        {
            Student selectedItem = dgS.SelectedItem as Student;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Student.Remove(selectedItem);

                    try
                    {
                        db.SaveChanges();
                        dgS.ItemsSource = db.Student.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления записи: Данный студент используется в другой таблице", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    MessageBox.Show("Запись успешно удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {
            Student selectedItem = dgS.SelectedItem as Student;

            if (selectedItem != null)
            {
                EditStudents editWindow = new EditStudents(selectedItem, db);

                editWindow.Show();
                this.Close();
            }
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            MainMenu mainWin = new MainMenu();
            mainWin.Show();
            this.Close();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbSearch.Text.ToLower();
            List<Student> filteredStudents = allStudent.Where(student =>
                student.SurnameStudent.ToLower().Contains(searchText) ||
                student.NameStudent.ToLower().Contains(searchText) ||
                student.PatronymicStudent.ToLower().Contains(searchText)
            ).ToList();

            dgS.ItemsSource = filteredStudents;
        }
    }
}
