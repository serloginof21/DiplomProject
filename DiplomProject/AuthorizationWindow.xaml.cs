using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public partial class AuthorizationWindow : Window
    {
        ChampionatEntities db;

        public AuthorizationWindow()
        {
            InitializeComponent();
            db = new ChampionatEntities();
        }

        private void Auth_ClickButton(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = tbPassword.Password;

            // Проверка наличия пользователя в базе данных
            if (CheckUser(login, password))
            {
                // Пользователь существует, открывается следующее окно
                MainMenu win1 = new MainMenu();
                win1.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь с указанными учетными данными не найден. Пожалуйста, зарегистрируйтесь.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckUser(string login, string password)
        {
            try
            {
                // Подключение к базе данных и выполнение запроса
                string connectionString = "Data Source=DESKTOP-5F950AM\\SQLEXPRESS;Initial Catalog=Championat;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE LoginUser = @Login AND PasswordUser = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);
                        int count = (int)command.ExecuteScalar();
                        return count > 0; // Если найдено совпадение, возвращает true
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Registration_ClickButton(object sender, RoutedEventArgs e)
        {
            Registration regWin = new Registration();
            regWin.Show();
            this.Close();
        }
    }
}
