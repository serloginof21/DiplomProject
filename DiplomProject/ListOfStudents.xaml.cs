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
    public partial class ListOfStudents : Window
    {
        ChampionatEntities db;

        public ListOfStudents()
        {
            InitializeComponent();
            db = new ChampionatEntities();
            LoadData();
            cb1.ItemsSource = db.Competences.ToList();
            cb2.ItemsSource = db.Category.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        private void LoadData()
        {
            var students = db.Student.Select(s => new StudentJuniorViewModel
            {
                Id = s.Id_Student,
                Surname = s.SurnameStudent,
                Name = s.NameStudent,
                Patronymic = s.PatronymicStudent,
                Email = s.EmailStudent,
                PhoneNumber = s.PhoneNumberStudent,
                Region = s.RegionStudent,
                Country = s.CountryStudent,
                Organization = s.Organizations.Organization,
                ClothingSize = s.ClothingSizes.NameSize,
                Category = s.Category.NameCategory,
                Competence = s.Competences.NameCompetence
            }).ToList();

            var juniors = db.Junior.Select(j => new StudentJuniorViewModel
            {
                Id = j.Id_Junior,
                Surname = j.SurnameJunior,
                Name = j.NameJunior,
                Patronymic = j.PatronymicJunior,
                Email = j.EmailJunior,
                PhoneNumber = j.PhoneNumberJunior,
                Region = j.RegionJunior,
                Country = j.CountryJunior,
                Organization = j.Organizations.Organization,
                ClothingSize = j.ClothingSizes.NameSize,
                Category = j.Category.NameCategory,
                Competence = j.Competences.NameCompetence
            }).ToList();

            var combinedList = students.Concat(juniors).ToList();
            dgList.ItemsSource = combinedList;
        }

        private void Export_ClickButton(object sender, RoutedEventArgs e)
        {
            string competenceName = cb1.SelectedItem != null ? ((Competences)cb1.SelectedItem).NameCompetence : "AllCompetences";
            string categoryName = cb2.SelectedItem != null ? ((Category)cb2.SelectedItem).NameCategory : "AllCategories";

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = $"{competenceName}_{categoryName}_ExportedData";
            dlg.Filter = "Excel documents (.xlsx)|*.xlsx";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    FileInfo newFile = new FileInfo(dlg.FileName);
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");

                        string[] headers = { "Фамилия", "Имя", "Отчество", "Адрес электронной почты",
                                     "Категория", "Регион", "Страна", "Номер телефона",
                                     "Организация", "Размер одежды"};

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = headers[i];
                        }

                        var students = (IEnumerable<StudentJuniorViewModel>)dgList.ItemsSource;

                        int row = 2;

                        foreach (var student in students)
                        {
                            worksheet.Cells[row, 1].Value = student.Surname;
                            worksheet.Cells[row, 2].Value = student.Name;
                            worksheet.Cells[row, 3].Value = student.Patronymic;
                            worksheet.Cells[row, 4].Value = student.Email;
                            worksheet.Cells[row, 5].Value = student.Category;
                            worksheet.Cells[row, 6].Value = student.Region;
                            worksheet.Cells[row, 7].Value = student.Country;
                            worksheet.Cells[row, 8].Value = student.PhoneNumber;
                            worksheet.Cells[row, 9].Value = student.Organization;
                            worksheet.Cells[row, 10].Value = student.ClothingSize;
                            row++;
                        }

                        var range = worksheet.Cells[1, 1, row - 1, headers.Length];
                        var tableBorder = range.Style.Border;
                        tableBorder.Bottom.Style = tableBorder.Top.Style = tableBorder.Left.Style = tableBorder.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        package.Save();
                    }

                    MessageBox.Show("Данные успешно экспортированы в Excel!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при экспорте данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
            Application.Current.Shutdown();
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterData();
        }

        private void cb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            int? selectedCompetenceId = cb1.SelectedItem != null ? (int?)((Competences)cb1.SelectedItem).Id_Competence : null;
            int? selectedCategoryId = cb2.SelectedItem != null ? (int?)((Category)cb2.SelectedItem).Id_Category : null;

            var filteredStudents = db.Student.AsQueryable();
            var filteredJuniors = db.Junior.AsQueryable();

            if (selectedCompetenceId.HasValue)
            {
                filteredStudents = filteredStudents.Where(student => student.Id_Competence == selectedCompetenceId.Value);
                filteredJuniors = filteredJuniors.Where(junior => junior.Id_Competence == selectedCompetenceId.Value);
            }

            if (selectedCategoryId.HasValue)
            {
                filteredStudents = filteredStudents.Where(student => student.Id_Category == selectedCategoryId.Value);
                filteredJuniors = filteredJuniors.Where(junior => junior.Id_Category == selectedCategoryId.Value);
            }

            var combinedList = filteredStudents.Select(s => new StudentJuniorViewModel
            {
                Id = s.Id_Student,
                Surname = s.SurnameStudent,
                Name = s.NameStudent,
                Patronymic = s.PatronymicStudent,
                Email = s.EmailStudent,
                PhoneNumber = s.PhoneNumberStudent,
                Region = s.RegionStudent,
                Country = s.CountryStudent,
                Organization = s.Organizations.Organization,
                ClothingSize = s.ClothingSizes.NameSize,
                Category = s.Category.NameCategory,
                Competence = s.Competences.NameCompetence
            }).ToList().Concat(filteredJuniors.Select(j => new StudentJuniorViewModel
            {
                Id = j.Id_Junior,
                Surname = j.SurnameJunior,
                Name = j.NameJunior,
                Patronymic = j.PatronymicJunior,
                Email = j.EmailJunior,
                PhoneNumber = j.PhoneNumberJunior,
                Region = j.RegionJunior,
                Country = j.CountryJunior,
                Organization = j.Organizations.Organization,
                ClothingSize = j.ClothingSizes.NameSize,
                Category = j.Category.NameCategory,
                Competence = j.Competences.NameCompetence
            }).ToList()).ToList();

            dgList.ItemsSource = combinedList;
        }

        private void Reset_ClickButton(object sender, RoutedEventArgs e)
        {
            cb1.SelectedIndex = -1;
            cb2.SelectedIndex = -1;
            LoadData();
        }
    }
}
