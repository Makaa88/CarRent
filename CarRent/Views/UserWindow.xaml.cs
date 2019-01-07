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
using CarRent.Models;
using CarRent.ViewModels;

namespace CarRent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private DatabaseConnection db;
        private MainWindow mainWindow;
        private MainViewModel model;
        public UserWindow()
        {
            InitializeComponent();
        }

        public UserWindow(ref DatabaseConnection db,int id, MainWindow window)
            :this()
        {
            this.db = db;
            this.mainWindow = window;
            model = new MainViewModel(ref this.db, id);
            DataContext = new MainView(model);
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Show();
        }

        private void MainPageButtonClick(object sender, RoutedEventArgs e)
        {
            DataContext = new MainView(model);
       
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
