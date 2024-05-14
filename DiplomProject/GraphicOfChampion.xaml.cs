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
        public GraphicOfChampion()
        {
            InitializeComponent();
        }

        private void Exit_ClickButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void cb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Reset_ClickButton(object sender, RoutedEventArgs e)
        {

        }

        private void Chooise_ClickButton(object sender, RoutedEventArgs e)
        {

        }
    }
}
