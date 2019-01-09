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
using CarRent.ViewModels;
using CarRent.Models;


namespace CarRent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy TravelView.xaml
    /// </summary>
    public partial class TravelView : UserControl
    {
        private TravelViewModel travelModel;
        private List<Car> cars;
        public TravelView()
        {
            InitializeComponent();
        }

        public TravelView(DatabaseConnection db, int id)
            :this()
        {
            travelModel = new TravelViewModel(db, id);
        }
         
        private void CoachButtonPressed(object sender, RoutedEventArgs e)
        {
            //cars = travelModel.GetCoaches();
            CarList.ItemsSource = travelModel.GetCoaches();// cars;
        }

        private void BusButtonClick(object sender, RoutedEventArgs e)
        {
            //cars = travelModel.GetBuses();
            CarList.ItemsSource = travelModel.GetBuses();//cars;
        }

    }
}
