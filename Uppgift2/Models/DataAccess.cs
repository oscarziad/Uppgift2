using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uppgift2.Models;
using Windows.Storage;

namespace Uppgift2
{
    public class DataAccess
    {
        public static Settings settings = new Settings();

        private static string connectionString = @"Data Source=DESKTOP-O844D6M\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";

        public static async void ReadSettingsAsync()
        {
            try
            {
                //Hämtar fil från Dockument
                StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
                StorageFile settingFile = await storageFolder.GetFileAsync(@"Settings.json");
                string json = await FileIO.ReadTextAsync(settingFile);
                settings = JsonConvert.DeserializeObject<Settings>(json);
            }
            catch { }
        }

        public static async Task AddAsync(Customer customer, Ticket ticket)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
               
                //Kontrollera om kunden redan finns i databasen
                using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from customers where SSNo = @SSNo", conn))
                {
                    conn.Open();
                    sqlCommand.Parameters.AddWithValue("@SSNo", customer.SSNo);

                    int customerInDb = (int)sqlCommand.ExecuteScalar();// hämtar kund

                    // om kunden finns lägg bara till ärende
                    if (customerInDb != 0)
                    {
                        var query = @"INSERT INTO Tickets (CustomerId,  Created, Status, Title, Category, Description)
                        VALUES(@CustomerId, @Created, @Status, @Title, @Category, @Description) ";

                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@CustomerId", customer.SSNo);
                        cmd.Parameters.AddWithValue("@Created", ticket.Created);
                        cmd.Parameters.AddWithValue("@Status", ticket.Status);
                        cmd.Parameters.AddWithValue("@Title", ticket.Title);
                        cmd.Parameters.AddWithValue("@Category", ticket.Category);
                        cmd.Parameters.AddWithValue("@Description", ticket.Description);
                       
                        await cmd.ExecuteReaderAsync();
                        conn.Close();
                    }
                    // om kund inte finns lägg till kund och ärende
                    else
                    {
                        var query = @"INSERT INTO Customers (SSNo, FirstName, LastName, PhoneNo, Email) 
                        VALUES(@SSNo, @FirstName, @LastName, @PhoneNo, @Email)
                        INSERT INTO Tickets (CustomerId, Created, Status, Title, Category, Description)
                        VALUES(@CustomerId, @Created, @Status, @Title, @Category, @Description) ";

                        SqlCommand cmd = new SqlCommand(query, conn);


                        cmd.Parameters.AddWithValue("@SSNo", customer.SSNo);
                        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
                        cmd.Parameters.AddWithValue("@Email", customer.Email);

                        cmd.Parameters.AddWithValue("@CustomerId", customer.SSNo);
                        cmd.Parameters.AddWithValue("@Created", ticket.Created);
                        cmd.Parameters.AddWithValue("@Status", ticket.Status);
                        cmd.Parameters.AddWithValue("@Title", ticket.Title);
                        cmd.Parameters.AddWithValue("@Category", ticket.Category);
                        cmd.Parameters.AddWithValue("@Description", ticket.Description);


                        await cmd.ExecuteReaderAsync();
                        conn.Close();
                    }
                }
            }

        }
        public static ObservableCollection<Ticket> GetAll(string status)
        {
            var customerList = new List<Customer>();
            var ticketList = new ObservableCollection<Ticket>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Hämta alla utifrån status
                var query = "SELECT * FROM Customers INNER JOIN Tickets ON Customers.SSNo = Tickets.CustomerId WHERE Tickets.Status = @Status";
                // Hämta alla utifrån kundid
                //var query = "SELECT * FROM Customers, Tickets WHERE Customers.SSNo = Tickets.CustomerId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);

                var result = cmd.ExecuteReader();

                while (result.Read())
                {
                    long SSNo = result.GetInt64(0);
                    string FirstName = result.GetString(1);
                    string LastName = result.GetString(2);
                    long Phone = result.GetInt64(3);
                    string Email = result.GetString(4);

                    int TicketId = result.GetInt32(5);
                    long CustomerId = result.GetInt64(6);
                    DateTime Created = result.GetDateTime(7);
                    string Status = result.GetString(8);
                    string Title = result.GetString(9);
                    string Category = result.GetString(10);
                    string Description = result.GetString(11);


                    Customer customer = new Customer(SSNo, FirstName, LastName, Phone, Email);
                    customerList.Add(customer);
                    ticketList.Add(new Ticket(TicketId, CustomerId, Created, Status, Title, Category, Description, customer));

                    //customerList.Add(new Customer(SSNo, FirstName, LastName, Phone, Email));
                    //ticketList.Add(new Ticket(TicketId, CustomerId, Created, Status, Title, Category, Description ));


                }

                conn.Close();
                return ticketList;
            }
        }
        public static ObservableCollection<Ticket> GetAllActive()
        {
            var customerList = new List<Customer>();
            var ticketList = new ObservableCollection<Ticket>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Hämta alla Pending och Active ärenden utifrån senaste skapta ärende id
                var query = "SELECT TOP (@Take) * FROM Customers INNER JOIN Tickets ON Customers.SSNo = Tickets.CustomerId WHERE Tickets.Status = @Status1 OR Tickets.Status = @Status2 ORDER BY Tickets.TicketId DESC";
           
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Take", settings.Take);
                cmd.Parameters.AddWithValue("@Status1", settings.Status[0]);
                cmd.Parameters.AddWithValue("@Status2", settings.Status[1]);

                var result = cmd.ExecuteReader();

                while (result.Read())
                {
                    long SSNo = result.GetInt64(0);
                    string FirstName = result.GetString(1);
                    string LastName = result.GetString(2);
                    long Phone = result.GetInt64(3);
                    string Email = result.GetString(4);

                    int TicketId = result.GetInt32(5);
                    long CustomerId = result.GetInt64(6);
                    DateTime Created = result.GetDateTime(7);
                    string Status = result.GetString(8);
                    string Title = result.GetString(9);
                    string Category = result.GetString(10);
                    string Description = result.GetString(11);



                    Customer customer = new Customer(SSNo, FirstName, LastName, Phone, Email);
                    customerList.Add(customer);
                    ticketList.Add(new Ticket(TicketId, CustomerId, Created, Status, Title, Category, Description, customer));


                }

                conn.Close();
                return ticketList;
            }
        }
        public static ObservableCollection<Ticket> GetAllClosed()
        {
            var customerList = new List<Customer>();
            var ticketList = new ObservableCollection<Ticket>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Hämta alla Closed ärenden utifrån senaste skapta ärende id
                var query = "SELECT TOP(@Take) * FROM Customers INNER JOIN Tickets ON Customers.SSNo = Tickets.CustomerId WHERE Tickets.Status = @Status ORDER BY Tickets.TicketId DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Take", settings.Take);
                cmd.Parameters.AddWithValue("@Status", settings.Status[2]);

                var result = cmd.ExecuteReader();

                while (result.Read())
                {
                    long SSNo = result.GetInt64(0);
                    string FirstName = result.GetString(1);
                    string LastName = result.GetString(2);
                    long Phone = result.GetInt64(3);
                    string Email = result.GetString(4);

                    int TicketId = result.GetInt32(5);
                    long CustomerId = result.GetInt64(6);
                    DateTime Created = result.GetDateTime(7);
                    string Status = result.GetString(8);
                    string Title = result.GetString(9);
                    string Category = result.GetString(10);
                    string Description = result.GetString(11);


                    Customer customer = new Customer(SSNo, FirstName, LastName, Phone, Email);
                    customerList.Add(customer);
                    ticketList.Add(new Ticket(TicketId, CustomerId, Created, Status, Title, Category, Description, customer));

                    //customerList.Add(new Customer(SSNo, FirstName, LastName, Phone, Email));
                    //ticketList.Add(new Ticket(TicketId, CustomerId, Created, Status, Title, Category, Description));


                }

                conn.Close();
                return ticketList;
            }
        }
        public static async Task UpdateAsync(int ticketId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Uppdaterar status utifrån ärende id
                var query = "UPDATE Tickets SET Status = @Status WHERE TicketId = @TicketId;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TicketId", ticketId);
                cmd.Parameters.AddWithValue("@Status", status);

                await cmd.ExecuteReaderAsync();
                conn.Close();
            }
        }

    }
}

