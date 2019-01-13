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

namespace CarRent.Views
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterUserView.xaml
    /// </summary>
    public partial class RegisterUserView : UserControl
    {
        private DatabaseConnection db;
        public RegisterUserView()
        {
            InitializeComponent();
        }

        public RegisterUserView(DatabaseConnection db)
            :this()
        {
            this.db = db;
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            if(AllLabelsFilled())
            {
                InfoLabel.Content = db.AddNewUser(NameLabel.Text, SurnameLabel.Text, Addressabel.Text, PhoneLabel.Text, MailLabel.Text, Password1Label.Password, Password2Label.Password);
            }
            else
            {
                InfoLabel.Content = "Wypełnij wszystkie pola";
            }

            NameLabel.Text = "";
            SurnameLabel.Text = "";
            Addressabel.Text = "";
            PhoneLabel.Text = "";
            MailLabel.Text = "";
            Password1Label.Password = "";
            Password2Label.Password = "";
        }

        private bool AllLabelsFilled()
        {
            if (String.IsNullOrEmpty(NameLabel.Text) || String.IsNullOrEmpty(SurnameLabel.Text) || String.IsNullOrEmpty(Addressabel.Text) || String.IsNullOrEmpty(PhoneLabel.Text) || String.IsNullOrEmpty(MailLabel.Text) || String.IsNullOrEmpty(Password1Label.Password) || String.IsNullOrEmpty(Password2Label.Password))
                return false;

            return true;
        }
    }
}
