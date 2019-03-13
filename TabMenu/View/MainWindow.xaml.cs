using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BancDelTemps.Model.Class;


namespace TabMenu
{
    /// <summary>
    /// Interraccio logica amb MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            

            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button) e.Source).Uid);

            GridCursor.Margin = new Thickness(10 + (150 * index), 45, 0, 7);
            GridMain.Visibility = Visibility.Hidden;
            GridUsers.Visibility = Visibility.Hidden;
            GridPosts.Visibility = Visibility.Hidden;
            GridReports.Visibility = Visibility.Hidden;
            GridPacts.Visibility = Visibility.Hidden;
            GridCategories.Visibility = Visibility.Hidden;

            switch (index)
            {
                case 0:
                   // GridMain.Background = Brushes.Aquamarine;
                    GridMain.Visibility = Visibility.Visible;
                    break;
                case 1:
                   // GridUsers.Background = Brushes.BurlyWood;
                    GridUsers.Visibility = Visibility.Visible;
                    break;
                case 2:
                   // GridPosts.Background = Brushes.CornflowerBlue;
                    GridPosts.Visibility = Visibility.Visible;
                    break;
                case 3:
                   // GridReports.Background = Brushes.Coral;
                    GridReports.Visibility = Visibility.Visible;
                    break;
                case 4:

                    //GridReports.Background = Brushes.HotPink;
                    GridPacts.Visibility = Visibility.Visible;
                    break;
                case 5:
                    GridCategories.Visibility = Visibility.Visible;
                   // GridMain.Background = Brushes.Gainsboro;
                    break;
                case 6:
                   // GridMain.Background = Brushes.HotPink;
                    break;
            }
        }


    }
}
