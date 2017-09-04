using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Kundregister
{
    class Program
    {
        static void Main(string[] args)
        {
            
            bool done = false;

            do
            {
                Menu();
            } while (!done);


        }

        private static void PrettyDisplayHeader()
        {
            Console.Write("Id".PadRight(5));
            Console.Write("FirstName".PadRight(15));
            Console.Write("LastName".PadRight(15));
            Console.Write("Email".PadRight(30));
            Console.WriteLine("Phone".PadRight(30));
            Console.WriteLine("-------------------------------------------------------------------");
        }

       

        private static void Menu()
        {
            Console.WriteLine("-----");
            Console.WriteLine("Menu: ");
            Console.WriteLine("-----");
            Console.WriteLine("[1] Add a new customer");
            Console.WriteLine("[2] Get data about a specific customer");
            Console.WriteLine("[3] Update data about an existing customer");
            Console.WriteLine("[4] Delete customer");
            Console.WriteLine("[5] Display data of all customers");
            Console.WriteLine("[6] Quit");
            Console.WriteLine();

            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddNewCustomer();
                    break;
                case 2:
                    RetrieveCustomerDataViaId();
                    break;
                case 3:
                    UppdateData();
                    break;
                case 4:
                    DeleteCustomer();
                    break;
                case 5:
                    DisplayCustomerFile();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private static void DisplayCustomerFile()
        {
            PrettyDisplayHeader();

            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM CustomerFile", con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.Write(reader.GetInt32(0).ToString().PadRight(5));
                            Console.Write(reader.GetString(1).ToString().PadRight(15));
                            Console.Write(reader.GetString(2).ToString().PadRight(15));
                            Console.Write(reader.GetString(3).ToString().PadRight(30));
                            Console.WriteLine(reader.GetString(4).ToString().PadRight(30));
                        }
                    }
                }
            }
        }

        private static void DeleteCustomer()
        {
            Console.Write("Delete customer with id: ");
            int customerId = int.Parse(Console.ReadLine());

            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM CustomerFile WHERE CustomerId = @CustomerId", con))
                {
                    command.Parameters.Add(new SqlParameter("CustomerId", customerId));
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void UppdateData()
        {
            Console.WriteLine("Update customer data");
            Console.Write("Customer id: ");
            int customerId = int.Parse(Console.ReadLine());


            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT  * FROM CustomerFile WHERE CustomerId = @CustomerId", con))
                {
                    command.Parameters.Add(new SqlParameter("CustomerId", customerId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}   {2}   {3}",
                                reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                        }
                    }

                }
                Console.Write("Choose which column to update (FirstName, LastName, Email, Phone)");
                string updateCol = Console.ReadLine();
                Console.Write("Replace with: ");
                string newData = Console.ReadLine();

                using (SqlCommand command = new SqlCommand($"UPDATE CustomerFile SET {updateCol} = @newData WHERE CustomerId = @CustomerId ", con))
                {
                    command.Parameters.Add(new SqlParameter("CustomerId", customerId));
                    command.Parameters.Add(new SqlParameter("newData", newData));
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void RetrieveCustomerDataViaId()
        {
            Console.Write("Customer id: ");
            int customerId = int.Parse(Console.ReadLine());


            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT  * FROM CustomerFile WHERE CustomerId LIKE @CustomerId", con))
                {
                    command.Parameters.Add(new SqlParameter("CustomerId", customerId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1,-10} {2,-10} {3,-15} {4,-20}",
                               reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                        }
                    }


                }
            }
        }

        private static void AddNewCustomer()
        {
            Console.WriteLine("Add a new customer:");
            Console.Write("First name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand($"INSERT INTO CustomerFile (FirstName, LastName, Email, Phone)" +
                                                           $"VALUES(@FirstName, @LastName, @Email, @Phone)", con))
                    {
                        command.Parameters.Add(new SqlParameter("FirstName", firstName));
                        command.Parameters.Add(new SqlParameter("LastName", lastName));
                        command.Parameters.Add(new SqlParameter("Email", email));
                        command.Parameters.Add(new SqlParameter("Phone", phone));
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Console.WriteLine("Could not insert.");
                }

            }
        }

    }
}

