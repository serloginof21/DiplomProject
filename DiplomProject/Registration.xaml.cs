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
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Registration_ClickButton(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqlConnection("Data Source=DESKTOP-5F950AM\\SQLEXPRESS;Initial Catalog=Championat;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    // Проверка существования пользователя с заданным логином
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE LoginUser = @LoginUser";
                    SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);
                    checkUserCommand.Parameters.AddWithValue("@LoginUser", tb1.Text);

                    int existingUserCount = (int)checkUserCommand.ExecuteScalar();

                    if (existingUserCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.");
                        return;
                    }

                    // Генерация случайного Id_User от 02 до 99
                    Random rnd = new Random();
                    int idUser = rnd.Next(2, 100);

                    // Проверка уникальности сгенерированного Id_User
                    string checkIdQuery = "SELECT COUNT(*) FROM Users WHERE Id_User = @IdUser";
                    SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection);
                    checkIdCommand.Parameters.AddWithValue("@IdUser", idUser);

                    int existingIdCount = (int)checkIdCommand.ExecuteScalar();

                    // Если сгенерированный id уже существует, генеририруется новый
                    while (existingIdCount > 0)
                    {
                        idUser = rnd.Next(1, 100);
                        checkIdCommand.Parameters["@IdUser"].Value = idUser;
                        existingIdCount = (int)checkIdCommand.ExecuteScalar();
                    }

                    // Создание команды SQL для вставки данных
                    string sql = "INSERT INTO Users (Id_User, LoginUser, PasswordUser) VALUES (@IdUser, @LoginUser, @PasswordUser)";
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.AddWithValue("@IdUser", idUser);
                    command.Parameters.AddWithValue("@LoginUser", tb1.Text);
                    command.Parameters.AddWithValue("@PasswordUser", tb2.Text);

                    // Выполнение команды SQL для вставки данных
                    command.ExecuteNonQuery();

                    MessageBox.Show("Пользователь успешно зарегистрирован!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при регистрации пользователя: " + ex.Message);
                }
            }
        }
    }
}
