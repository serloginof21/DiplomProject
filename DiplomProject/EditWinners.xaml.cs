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
    public partial class EditWinners : Window
    {
        private StudentWinner selectedWinner;
        public ChampionatEntities db;

        public EditWinners(StudentWinner studentWinner, ChampionatEntities db)
        {
            InitializeComponent();
            this.selectedWinner = studentWinner;
            this.db = db;
            tbId.Text = selectedWinner.Id_Winner.ToString();
            dt1.Text = selectedWinner.DateOfWin.ToString();

            cb1.ItemsSource = db.Student.ToList();
            cb1.SelectedItem = db.Student.FirstOrDefault(o => o.SurnameStudent == selectedWinner.Student.SurnameStudent);

            cb2.ItemsSource = db.PlaceOfWinners.ToList();
            cb2.SelectedItem = db.PlaceOfWinners.FirstOrDefault(o => o.NamePlace == selectedWinner.PlaceOfWinners.NamePlace);

            cb3.ItemsSource = db.CampionatStages.ToList();
            cb3.SelectedItem = db.CampionatStages.FirstOrDefault(o => o.NameStage == selectedWinner.CampionatStages.NameStage);
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                GraphicOfWinners winnerWindow = new GraphicOfWinners();
                winnerWindow.Show();
                this.Close();
            }
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {
            if (!FieldsAreValid())
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                selectedWinner.Id_WinnerStudent = ((Student)cb1.SelectedItem).Id_Student;
                selectedWinner.Id_WinnerPlace = ((PlaceOfWinners)cb2.SelectedItem).Id_Place;
                selectedWinner.Id_ChampionStage = ((CampionatStages)cb3.SelectedItem).Id_Stage;

                selectedWinner.DateOfWin = dt1.SelectedDate.Value;

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

        private bool FieldsAreValid()
        {
            return !string.IsNullOrWhiteSpace(tbId.Text) &&
                   !string.IsNullOrWhiteSpace(dt1.Text) &&
                   cb1.SelectedItem != null &&
                   cb2.SelectedItem != null &&
                   cb3.SelectedItem != null;
        }
    }
}
