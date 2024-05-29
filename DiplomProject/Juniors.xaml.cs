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
    public partial class Juniors : Window
    {
        ChampionatEntities db;
        private List<Junior> allJunior;
        public Juniors()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            allJunior = db.Junior.ToList();
            dgJ.ItemsSource = db.Junior.ToList();
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            AddJunior junWin = new AddJunior();
            junWin.Show();
            this.Close();
        }

        private void Delete_ClickButton(object sender, RoutedEventArgs e)
        {
            Junior selectedItem = dgJ.SelectedItem as Junior;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Junior.Remove(selectedItem);

                    try
                    {
                        db.SaveChanges();
                        dgJ.ItemsSource = db.Junior.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    MessageBox.Show("Запись успешно удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {
            Junior selectedItem = dgJ.SelectedItem as Junior;

            if (selectedItem != null)
            {
                EditJunior editWindow = new EditJunior(selectedItem, db);

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
            List<Junior> filteredJuniors = allJunior.Where(junior =>
                junior.SurnameJunior.ToLower().Contains(searchText) ||
                junior.NameJunior.ToLower().Contains(searchText) ||
                junior.PatronymicJunior.ToLower().Contains(searchText)
            ).ToList();
            dgJ.ItemsSource = filteredJuniors;
        }
    }
}
