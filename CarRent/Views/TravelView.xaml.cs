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
            if(CheckIfAllFieldsFilled())
            {
                ErrorLabel.Content = "Tak";
            }
            else
            {
                ErrorLabel.Content = "Nie";
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
