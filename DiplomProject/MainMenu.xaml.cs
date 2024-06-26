﻿using System;
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
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Students_ClickButton(object sender, RoutedEventArgs e)
        {
            Students stWin = new Students();
            stWin.Show();
            this.Close();
        }

        private void Experts_ClickButton(object sender, RoutedEventArgs e)
        {
            WindowExperts expertWin = new WindowExperts();
            expertWin.Show();
            this.Close();
        }

        private void ListOfStudents_ClickButton(object sender, RoutedEventArgs e)
        {
            ListOfStudents listWin = new ListOfStudents();
            listWin.Show();
            this.Close();
        }

        private void GraphisOfChampion_ClickButton(object sender, RoutedEventArgs e)
        {
            GraphicOfChampion grWin = new GraphicOfChampion();
            grWin.Show();
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

        private void Juniors_ClickButton(object sender, RoutedEventArgs e)
        {
            Juniors junWin = new Juniors();
            junWin.Show();
            this.Close();
        }

        private void Winners_ClickButton(object sender, RoutedEventArgs e)
        {
            GraphicOfWinners graphicOfWinners = new GraphicOfWinners();
            graphicOfWinners.Show();
            this.Close();
        }

        private void Back_ClickButton(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                authorizationWindow.Show();
                this.Close();
            }
        }
    }
}
