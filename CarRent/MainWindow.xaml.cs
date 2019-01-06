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
using CarRent.Models;
using CarRent.Views;
using System.ComponentModel;

namespace CarRent
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {

        private DatabaseConnection db = new DatabaseConnection();
        public MainWindow()
        {
            InitializeComponent();
            db.OpenDataBase();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //DatabaseConnection conn = new DatabaseConnection(LoginBox.Text, PasswordBox.Password,ref TestLabel);
            //  DataContext = new MainView();
            if(!db.LogIn(LoginBox.Text, PasswordBox.Password))
            {
                ErrorLabel.Content = "Błędne dane";
                ErrorLabel.Foreground = new SolidColorBrush(Colors.Red);

            }
            else
            {
                DataContext = new MainView(ref db);
            }
        }

        void WindowClosing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("ZAKYKANUE");
            db.Close();
        }
    }
}
