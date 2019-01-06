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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarRent.Models;
using CarRent.ViewModels;

namespace CarRent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private MainViewModel model;
        public MainView(ref DatabaseConnection db)
            :this()
        {
            model = new MainViewModel(ref db);
            
        }

        public MainView()
        {
            InitializeComponent();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            model.CloseConnection();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
