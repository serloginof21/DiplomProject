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
                bool isUsed = db.ChampionSchedule.Any(x => x.Id_ChiefExpert == selectedItem.Id_Expert || x.Id_MentorExpert == selectedItem.Id_Expert || x.Id_TechnicalExpert == selectedItem.Id_Expert);

                if (isUsed)
                {
                    MessageBox.Show("Данный эксперт используется в другой таблице!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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
                        MessageBox.Show($"Этот эксперт используется в другой таблице", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    MessageBox.Show("Запись успешно удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            List<Expert> filteredExperts = allExperts.Where(expert =>
                expert.SurnameExpert.ToLower().Contains(searchText) ||
                expert.NameExpert.ToLower().Contains(searchText) ||
                expert.PatronymicExpert.ToLower().Contains(searchText)
            ).ToList();
            dgE.ItemsSource = filteredExperts;
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
