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
    /// Logika interakcji dla klasy OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        private OrderViewModel orderViewModel;
        private bool isCancelButtonAllowedToClick = false;
        public OrdersView()
        {
            InitializeComponent();
        }

        public OrdersView(DatabaseConnection db, int userId)
            : this()
        {
            orderViewModel = new OrderViewModel(db, userId);
            CancelOrderButton.IsEnabled = false;
        }

        private void OrdersDoneButtonClick(object sender, RoutedEventArgs e)
        {
            OrderList.ItemsSource = orderViewModel.GetOrders(1);
            CancelOrderButton.IsEnabled = false;
            isCancelButtonAllowedToClick = false;
        }

        private void OrderInRealizationButtonClick(object sender, RoutedEventArgs e)
        {
            OrderList.ItemsSource = orderViewModel.GetOrders(3);
            CancelOrderButton.IsEnabled = false;
            isCancelButtonAllowedToClick = true;
        }

        private void OrderListSelectionItemChanged(object sender, SelectionChangedEventArgs e)
        {
            if(isCancelButtonAllowedToClick) 
             CancelOrderButton.IsEnabled = true;
        }

        private void CancelOrderButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrderList.SelectedItems[0] is OrderDetails order)
                {
                    OrderList.ItemsSource = orderViewModel.DeleteOrder(order.OrderID);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }
    }
}
