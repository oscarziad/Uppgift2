using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift2.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public long CustomerId { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public string Summery => $"{TicketId}, {CustomerId}, {Created}, {Status}, {Title}, {Category}, {Description}";
        public virtual Customer customer { get; set; } // virtual skapar temporärt behöver inte sättas med värden


        public Ticket()
        {
                
        }

        public Ticket (string title, string category, string description)
        {
            Created = DateTime.Now;
            Status = "Pending";
            Title = title;
            Category = category;
            Description = description;
    
        }

       
        public Ticket(int ticketId, long customerId, DateTime created, string status, string title, string category, string description, Customer customer)
        {
            TicketId = ticketId; 
            CustomerId = customerId; 
            Created = created;
            Status = status;
            Title = title;
            Category = category;
            Description = description;
            this.customer = customer;
        }

        public class TicketViewModel
        {
            public ObservableCollection<Ticket> Tickets { get; set; }

            public TicketViewModel()
            {
                
            }

            public void GetActive()
            {
                Tickets = DataAccess.GetAllActive();
            }
            public void GetClosed()
            {
                Tickets = DataAccess.GetAllClosed();
            }
        }

    }
}
