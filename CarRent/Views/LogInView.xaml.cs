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
    /// Logika interakcji dla klasy LogInView.xaml
    /// </summary>
    public partial class LogInView : UserControl
    {
        private DatabaseConnection db;
        MainWindow window;
        int userID = 0;
        public LogInView()
        {
            InitializeComponent();
        }

        public LogInView(int result, DatabaseConnection db, MainWindow wind)
            : this()
        {
            if(result == -1)
            {
                ErrorLabel.Content = "Nie udało się połączyć";
            }
            else
            {
                this.db = db;
                this.window = wind;
            }
        }


        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            this.userID = db.LogIn(LoginBox.Text, PasswordBox.Password);
            if (this.userID == 0)
            {
                ErrorLabel.Content = "Błędne dane";
                ErrorLabel.Foreground = new SolidColorBrush(Colors.Red);

            }
            else if (this.userID == -1)
            {
                ErrorLabel.Content = "Brak połączenia";
            }
            else
            {
                var window = new UserWindow(ref db, userID, this.window);
                window.Show();
                this.window.Hide();
                LoginBox.Clear();
                PasswordBox.Clear();
                ErrorLabel.Content = "";
            }
        }
    }
}
