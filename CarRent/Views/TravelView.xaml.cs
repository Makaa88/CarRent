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
            CarList.ItemsSource = travelModel.GetCoaches();
        }

        private void BusButtonClick(object sender, RoutedEventArgs e)
        {
            CarList.ItemsSource = travelModel.GetBuses();
        }

        private void AddTravelButtonClick(object sender, RoutedEventArgs e)
        {
            int id = -1;
            try
            {
                if (CarList.SelectedItems[0] is Car car)
                {
                    id = car.CarID;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                ErrorLabel.Content = "Wybierz pojazd!";
                return;
            }


            if (CheckIfAllFieldsFilled())
            {
                int res = travelModel.ArrangeTravel(id, StartCountryText.Text, StartTownText.Text, StartStreetText.Text, StartDate.Text, StartHour.Text, EndCountryText.Text, EndTownText.Text, EndStreetText.Text, EndDate.Text, EndHour.Text);
                if(res == -1)
                {
                    ErrorLabel.Content = "Nie udało się dodać zamówienia!";
                }
                else if(res == -2)
                {
                    ErrorLabel.Content = "Podaj prawidłową date";
                }
                else if(res == -3)
                {
                    ErrorLabel.Content = "Kod błedu -3";
                }
                else if (res == -4)
                {
                    ErrorLabel.Content = "Kod błedu -4";
                }
                else if (res == -5)
                {
                    ErrorLabel.Content = "Kod błedu -5";
                }
                else if(res == -6)
                {
                    ErrorLabel.Content = "Podrózne można zamiawiać najwyżej\njeden dzień przed wyjazdem";
                }
                else
                {
                    StartTownText.Text = EndTownText.Text = "";
                    StartStreetText.Text = EndStreetText.Text = "";
                    StartDate.Text = EndDate.Text = "";
                    StartHour.Text = EndHour.Text = "";
                    CarList.ItemsSource = travelModel.GetCoaches();
                    ErrorLabel.Content = "Dodano zamównienie";
                }
            }
            else
            {
                ErrorLabel.Content = "Uzupełnij dane podrózy";
            }
        }



        //TODO figure out how to move this to travelModel
        private bool CheckIfAllFieldsFilled()
        {
            if (String.IsNullOrEmpty(StartCountryText.Text) || String.IsNullOrEmpty(StartTownText.Text) || String.IsNullOrEmpty(StartStreetText.Text) || String.IsNullOrEmpty(StartDate.Text) || String.IsNullOrEmpty(StartHour.Text))
                return false;
            if (String.IsNullOrEmpty(EndCountryText.Text) || String.IsNullOrEmpty(EndTownText.Text) || String.IsNullOrEmpty(EndStreetText.Text) || String.IsNullOrEmpty(EndDate.Text) || String.IsNullOrEmpty(EndHour.Text))
                return false;
            return true;
        }

       
    }
}
