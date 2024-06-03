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
using LiveCharts;
using LiveCharts.Wpf;

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
            cb3.ItemsSource = db.PlaceOfWinners.ToList();

            UpdatePlot();
        }

        private void UpdatePlot()
        {
            int selectedStageId = cb1.SelectedItem != null ? (cb1.SelectedItem as CampionatStages).Id_Stage : 0;
            int selectedPlaceId = cb3.SelectedItem != null ? (cb3.SelectedItem as PlaceOfWinners).Id_Place : 0;

            var data = db.StudentWinner.AsQueryable();

            if (selectedStageId > 0)
            {
                data = data.Where(w => w.Id_ChampionStage == selectedStageId);
            }

            if (selectedPlaceId > 0)
            {
                data = data.Where(w => w.Id_WinnerPlace == selectedPlaceId);
            }

            var groupedData = data
                .GroupBy(w => w.DateOfWin.Year)
                .Select(g => new { Year = g.Key, Count = g.Count() })
                .OrderBy(d => d.Year)
                .ToList();

            var values = new ChartValues<int>();
            var labels = new string[groupedData.Count];

            for (int i = 0; i < groupedData.Count; i++)
            {
                values.Add(groupedData[i].Count);
                labels[i] = groupedData[i].Year.ToString();
            }

            cartesianChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Количество призовых мест",
                    Values = values
                }
            };

            cartesianChart.AxisX.Clear();
            cartesianChart.AxisX.Add(new Axis
            {
                Title = "Год",
                Labels = labels
            });

            cartesianChart.AxisY.Clear();
            cartesianChart.AxisY.Add(new Axis
            {
                Title = "Количество призовых мест",
                LabelFormatter = value => value.ToString("N")
            });
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
                    UpdatePlot();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {

            MainMenu mainWin = new MainMenu();
            mainWin.Show();
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlot();
        }

        private void Reset_ClickButton(object sender, RoutedEventArgs e)
        {
            cb1.SelectedItem = null;
            cb3.SelectedItem = null;
            UpdatePlot();
        }
    }
}
