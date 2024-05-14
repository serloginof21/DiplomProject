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
            dgList.ItemsSource = db.Student.ToList();
            cb1.ItemsSource = db.Competences.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Export_ClickButton(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = cb1.SelectedItem != null ? ((Competences)cb1.SelectedItem).NameCompetence : "ExportedData"; 
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

                        var students = (IEnumerable<Student>)dgList.ItemsSource;

                        int row = 2;

                        foreach (var student in students)
                        {
                            worksheet.Cells[row, 1].Value = student.SurnameStudent;
                            worksheet.Cells[row, 2].Value = student.NameStudent;
                            worksheet.Cells[row, 3].Value = student.PatronymicStudent;
                            worksheet.Cells[row, 4].Value = student.EmailStudent;
                            worksheet.Cells[row, 5].Value = student.Category.NameCategory;
                            worksheet.Cells[row, 6].Value = student.Organizations.Region;
                            worksheet.Cells[row, 7].Value = student.Organizations.Country;
                            worksheet.Cells[row, 8].Value = student.PhoneNumberStudent;
                            worksheet.Cells[row, 9].Value = student.Organizations.Organization;
                            worksheet.Cells[row, 10].Value = student.ClothingSizes.NameSize;
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
            if (cb1.SelectedItem != null)
            {
                int selectedCompetenceId = ((Competences)cb1.SelectedItem).Id_Competence;
                var filteredStudents = db.Student.Where(student => student.Id_Competence == selectedCompetenceId).ToList();
                dgList.ItemsSource = filteredStudents;
            }
        }

        private void Reset_ClickButton(object sender, RoutedEventArgs e)
        {
            cb1.SelectedIndex = -1;
            dgList.ItemsSource = db.Student.ToList();
        }
    }
}
