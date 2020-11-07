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
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uppgift2.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenCasesPage : Page
    {
        public ObservableCollection<Ticket> Tickets { get; set; }
        public OpenCasesPage()
        {
            this.InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            Ticket.TicketViewModel ticketViewModel = new Ticket.TicketViewModel();
            ticketViewModel.GetActive();
            Tickets = ticketViewModel.Tickets;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await DataAccess.UpdateAsync(Convert.ToInt32(TicketId.Text), "Closed");
             LoadData();
            TicketCollection.Source = Tickets;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            await DataAccess.UpdateAsync(Convert.ToInt32(TicketId.Text), "Active");
            LoadData();
            TicketCollection.Source = Tickets;
            
        }
    }
}
