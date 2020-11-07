using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uppgift2.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uppgift2.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTicketView : Page
    {
        public AddTicketView()
        {
            this.InitializeComponent();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer(Convert.ToInt64(tbxSsno.Text),tbxFirstName.Text, tbxLastName.Text, Convert.ToInt64(tbxPhoneNumber.Text), tbxMailAdress.Text);
            Ticket ticket = new Ticket(tbxTitel.Text, cmbCategory.Text, tbxDescription.Text);

            await DataAccess.AddAsync(customer, ticket);

            tbxSsno.Text = "";
            tbxFirstName.Text = "";
            tbxLastName.Text = "";
            tbxPhoneNumber.Text = "";
            tbxMailAdress.Text = "";
            tbxTitel.Text = "";
            tbxDescription.Text = "";
            cmbCategory.SelectedIndex = -1;
                
        }
    }
}
