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
            int result = db.OpenDataBase();
            DataContext = new LogInView(result, db,this);
            /*if(result == -1)
            {
                ErrorLabel.Content = "Nie udało się połączyć!";
            }*/
        }

       /* private void Button_Click(object sender, RoutedEventArgs e)
        {
            userID = db.LogIn(LoginBox.Text, PasswordBox.Password);
             if (userID == 0)
             {
                 ErrorLabel.Content = "Błędne dane";
                 ErrorLabel.Foreground = new SolidColorBrush(Colors.Red);

             }
             else if(userID == -1)
             {
                ErrorLabel.Content = "Brak połączenia";
             }
             else
             {
                var window = new UserWindow(ref db,userID,this);
                window.Show();
                this.Hide();
                LoginBox.Clear();
                PasswordBox.Clear();
                ErrorLabel.Content = "";
             }
        }*/

        void WindowClosing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("ZAKYKANUE");
            db.Close();
        }

        private void AddNewUserButtonClick(object sender, RoutedEventArgs e)
        {
            DataContext = new RegisterUserView(db);
        }

        private void LogInFormButtonClick(object sender, RoutedEventArgs e)
        {
            DataContext = new LogInView(1, db, this);
        }
    }
}
