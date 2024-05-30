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
    public partial class AddWinner : Window
    {
        private Random random = new Random();
        ChampionatEntities db;

        public AddWinner()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            cb1.ItemsSource = db.Student.ToList();
            cb2.ItemsSource = db.PlaceOfWinners.ToList();
            cb3.ItemsSource = db.CampionatStages.ToList();
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            if (!FieldsAreValid())
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            try
            {
                StudentWinner sWinner = new StudentWinner();
                sWinner.Id_Winner = Convert.ToInt32(tbId.Text);
                sWinner.DateOfWin = dt1.SelectedDate.Value;


                Student selectedStudent = cb1.SelectedItem as Student;
                if (selectedStudent != null)
                {
                    sWinner.Id_WinnerStudent = selectedStudent.Id_Student;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите студента.");
                    return;
                }

                PlaceOfWinners selectedPlace = cb2.SelectedItem as PlaceOfWinners;
                if (selectedPlace != null)
                {
                    sWinner.Id_WinnerPlace = selectedPlace.Id_Place;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите место.");
                    return;
                }

                CampionatStages selectedStage = cb3.SelectedItem as CampionatStages;
                if (selectedStage != null)
                {
                    sWinner.Id_ChampionStage = selectedStage.Id_Stage;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите размер одежды.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите добавить новую запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.StudentWinner.Add(sWinner);
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены в базу данных.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных в базу данных: {ex.Message}");
            }
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            GraphicOfWinners winnerWindow = new GraphicOfWinners();
            winnerWindow.Show();
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
            } while (db.StudentWinner.Any(p => p.Id_Winner == randomID));
            return newID;
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
