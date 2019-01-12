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
    /// Logika interakcji dla klasy PasswordChangeView.xaml
    /// </summary>
    public partial class PasswordChangeView : UserControl
    {
        private PasswordChangeModel passwordModel;

        public PasswordChangeView()
        {
            InitializeComponent();
        }

        public PasswordChangeView(DatabaseConnection db, int id)
            : this()
        {
            passwordModel = new PasswordChangeModel(db, id);
        }

        private void ChangePasswordButtonClick(object sender, RoutedEventArgs e)
        {
            ContentLabel.Content = "";
            ContentLabel.Content = passwordModel.ChangePassword(OldPasswordBox.Password, FirstNewPasswordBox.Password, SecondNewPasswordBox.Password);
            OldPasswordBox.Password = "";
            FirstNewPasswordBox.Password = "";
            SecondNewPasswordBox.Password = "";
            Console.WriteLine("kdsjklfnlksad");
        }
    }
}
