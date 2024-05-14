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
    public partial class GraphicOfChampion : Window
    {
        ChampionatEntities db;
        private Random random = new Random();
        public GraphicOfChampion()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            dgG.ItemsSource = db.ChampionSchedule.ToList();
            cb1.ItemsSource = db.Competences.ToList();
            cb2.ItemsSource = db.Expert.ToList();
            cb3.ItemsSource = db.Expert.ToList();
            cb4.ItemsSource = db.Expert.ToList();

            cb2.IsEnabled = false;
            cb3.IsEnabled = false;
            cb4.IsEnabled = false;

            // Привязыв обработчика события SelectionChanged для cb1
            cb1.SelectionChanged += cb1_SelectionChanged;
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            if (cb1.SelectedItem == null || cb2.SelectedItem == null || cb3.SelectedItem == null || cb4.SelectedItem == null || string.IsNullOrEmpty(tbId.Text) || dt1.SelectedDate == null || dt2.SelectedDate == null || dt3.SelectedDate == null || dt4.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы хотите внести изменения в график?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ChampionSchedule newSchedule = new ChampionSchedule
                    {
                        Id_Schedule = int.Parse(tbId.Text),
                        Id_Competence = ((Competences)cb1.SelectedItem).Id_Competence,
                        Id_ChiefExpert = ((Expert)cb2.SelectedItem).Id_Expert,
                        Id_MentorExpert = ((Expert)cb3.SelectedItem).Id_Expert,
                        Id_TechnicalExpert = ((Expert)cb4.SelectedItem).Id_Expert,
                        MainGroupStartDate = dt1.SelectedDate.Value,
                        MainGroupEndDate = dt2.SelectedDate.Value,
                        JuniorStartDate = dt3.SelectedDate.Value,
                        JuniorEndDate = dt4.SelectedDate.Value
                    };

                    db.ChampionSchedule.Add(newSchedule);
                    db.SaveChanges();

                    MessageBox.Show("Изменения в графике успешно внесены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgG.ItemsSource = db.ChampionSchedule.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Edit_ClickButton(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_ClickButton(object sender, RoutedEventArgs e)
        {

        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            MainMenu mainWin = new MainMenu();
            mainWin.Show();
            this.Close();
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            } while (db.ChampionSchedule.Any(p => p.Id_Schedule == randomID));
            return newID;
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Competences selectedCompetence = cb1.SelectedItem as Competences;

            if (selectedCompetence != null)
            {
                var filteredExperts = selectedCompetence.Expert.ToList();

                var mainExperts = filteredExperts.Where(expert => expert.ExpertRole.NameRole == "Главный эксперт").ToList();
                var expertMethodologists = filteredExperts.Where(expert => expert.ExpertRole.NameRole == "Эксперт-наставник").ToList();
                var technicalExperts = filteredExperts.Where(expert => expert.ExpertRole.NameRole == "Технический администратор площадки").ToList();

                cb2.ItemsSource = mainExperts;
                cb3.ItemsSource = expertMethodologists;
                cb4.ItemsSource = technicalExperts;

                cb2.IsEnabled = true;
                cb3.IsEnabled = true;
                cb4.IsEnabled = true;
            }
            else
            {
                cb2.IsEnabled = false;
                cb3.IsEnabled = false;
                cb4.IsEnabled = false;
            }
        }
    }
}
