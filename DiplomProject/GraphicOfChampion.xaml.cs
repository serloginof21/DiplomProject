using OfficeOpenXml;
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
using OfficeOpenXml;
using System.IO;

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

            cb1.SelectionChanged += cb1_SelectionChanged;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Add_ClickButton(object sender, RoutedEventArgs e)
        {
            if (cb1.SelectedItem == null || cb2.SelectedItem == null || cb3.SelectedItem == null || cb4.SelectedItem == null 
                || string.IsNullOrEmpty(tbId.Text) || dt1.SelectedDate == null || dt2.SelectedDate == null 
                || dt3.SelectedDate == null || dt4.SelectedDate == null)
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
            if (dgG.SelectedItem != null)
            {
                ChampionSchedule selectedSchedule = dgG.SelectedItem as ChampionSchedule;
                if (selectedSchedule != null)
                {
                    selectedSchedule.Id_Schedule = int.Parse(tbId.Text);
                    selectedSchedule.Competences = cb1.SelectedItem as Competences;
                    selectedSchedule.Expert = cb2.SelectedItem as Expert;
                    selectedSchedule.Expert1 = cb3.SelectedItem as Expert;
                    selectedSchedule.Expert2 = cb4.SelectedItem as Expert;
                    selectedSchedule.MainGroupStartDate = dt1.SelectedDate.HasValue ? dt1.SelectedDate.Value : default(DateTime);
                    selectedSchedule.MainGroupEndDate = dt2.SelectedDate.HasValue ? dt2.SelectedDate.Value : default(DateTime);
                    selectedSchedule.JuniorStartDate = dt3.SelectedDate.HasValue ? dt3.SelectedDate.Value : default(DateTime);
                    selectedSchedule.JuniorEndDate = dt4.SelectedDate.HasValue ? dt4.SelectedDate.Value : default(DateTime);

                    db.SaveChanges();
                    dgG.Items.Refresh();
                    MessageBox.Show("Изменения в графике сохранены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент для редактирования.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Delete_ClickButton(object sender, RoutedEventArgs e)
        {
            if (dgG.SelectedItem != null)
            {
                ChampionSchedule selectedSchedule = dgG.SelectedItem as ChampionSchedule;
                if (selectedSchedule != null)
                {
                    db.ChampionSchedule.Remove(selectedSchedule);
                    db.SaveChanges();
                    dgG.ItemsSource = db.ChampionSchedule.ToList();
                    MessageBox.Show("Позиция удалена из графика.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            MainMenu mainWin = new MainMenu();
            mainWin.Show();
            this.Close();
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
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

        private void dgG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgG.SelectedItem != null)
            {
                ChampionSchedule selectedSchedule = dgG.SelectedItem as ChampionSchedule;
                if (selectedSchedule != null)
                {
                    tbId.Text = selectedSchedule.Id_Schedule.ToString();
                    cb1.SelectedItem = selectedSchedule.Competences;
                    cb2.SelectedItem = selectedSchedule.Expert;
                    cb3.SelectedItem = selectedSchedule.Expert1;
                    cb4.SelectedItem = selectedSchedule.Expert2;
                    dt1.SelectedDate = selectedSchedule.MainGroupStartDate;
                    dt2.SelectedDate = selectedSchedule.MainGroupEndDate;
                    dt3.SelectedDate = selectedSchedule.JuniorStartDate;
                    dt4.SelectedDate = selectedSchedule.JuniorEndDate;
                }
            }
        }

        private void Export_ClickButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    FilterIndex = 1,
                    FileName = "График проведения чемпионата"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileInfo newFile = new FileInfo(saveFileDialog.FileName);

                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("График");

                        worksheet.Cells[1, 1].Value = "Компетенция";
                        worksheet.Cells[1, 2].Value = "Главный эксперт";
                        worksheet.Cells[1, 3].Value = "Эксперт-наставник";
                        worksheet.Cells[1, 4].Value = "Технический эксперт";
                        worksheet.Cells[1, 5].Value = "Основа";
                        worksheet.Cells[1, 6].Value = "Юниоры";

                        for (int i = 0; i < dgG.Items.Count; i++)
                        {
                            var item = dgG.Items[i] as ChampionSchedule;

                            worksheet.Cells[i + 2, 1].Value = item.Competences.NameCompetence;
                            worksheet.Cells[i + 2, 2].Value = item.Expert?.FullName;
                            worksheet.Cells[i + 2, 3].Value = item.Expert1?.FullName;
                            worksheet.Cells[i + 2, 4].Value = item.Expert2?.FullName;
                            worksheet.Cells[i + 2, 5].Value = $"{item.MainGroupStartDate:dd.MM} - {item.MainGroupEndDate:dd.MM}";
                            worksheet.Cells[i + 2, 6].Value = $"{item.JuniorStartDate:dd.MM} - {item.JuniorEndDate:dd.MM}";
                        }

                        worksheet.Cells[1, 1, dgG.Items.Count + 1, 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 1, dgG.Items.Count + 1, 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 1, dgG.Items.Count + 1, 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 1, dgG.Items.Count + 1, 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        package.Save();
                    }

                    MessageBox.Show("Экспорт завершен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
