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
using OxyPlot;
using OxyPlot.Series;

namespace DiplomProject
{
    public partial class GraphicOfWinners : Window
    {
        ChampionatEntities db;

        public GraphicOfWinners()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            dgW.ItemsSource = db.StudentWinner.ToList();
            cb1.ItemsSource = db.CampionatStages.ToList();
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            AddWinner winnerWindow = new AddWinner();
            winnerWindow.Show();
            this.Close();
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {
            StudentWinner selectedItem = dgW.SelectedItem as StudentWinner;

            if (selectedItem != null)
            {
                EditWinners editWindow = new EditWinners(selectedItem, db);

                editWindow.Show();
                this.Close();
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentWinner selectedItem = dgW.SelectedItem as StudentWinner;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.StudentWinner.Remove(selectedItem);

                    try
                    {
                        db.SaveChanges();
                        dgW.ItemsSource = db.StudentWinner.ToList();
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

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainMenu mainWin = new MainMenu();
                mainWin.Show();
                this.Close();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
