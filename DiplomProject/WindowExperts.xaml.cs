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
    public partial class WindowExperts : Window
    {
        public ChampionatEntities db;
        private List<Expert> allExperts;

        public WindowExperts()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            dgE.ItemsSource = db.Expert.ToList();
            allExperts = db.Expert.ToList();
            tbSearch.TextChanged += tbSearch_TextChanged;
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            AddExpert addExWin = new AddExpert();
            addExWin.Show();
            this.Close();
        }

        private void Delete_ClickButton(object sender, RoutedEventArgs e)
        {
            Expert selectedItem = dgE.SelectedItem as Expert;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Expert.Remove(selectedItem);

                    try
                    {
                        db.SaveChanges();
                        dgE.ItemsSource = db.Expert.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            MainMenu mainWin = new MainMenu();
            mainWin.Show();
            this.Close();
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {
            Expert selectedItem = dgE.SelectedItem as Expert;

            if (selectedItem != null)
            {
                EditExperts editEWindow = new EditExperts(selectedItem, db);
                editEWindow.Show();
                this.Close();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbSearch.Text.ToLower();
            List<Expert> filteredParticipants = allExperts.Where(participant =>
                participant.SurnameExpert.ToLower().Contains(searchText) ||
                participant.NameExpert.ToLower().Contains(searchText) ||
                participant.PatronymicExpert.ToLower().Contains(searchText)
            ).ToList();
            dgE.ItemsSource = filteredParticipants;
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
