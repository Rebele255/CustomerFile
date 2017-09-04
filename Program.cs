using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using UsefulMethods;
using System.Text.RegularExpressions;

namespace Kundregister
{
    class Program
    {
        static Methods printer = new Methods();

        static void Main(string[] args)
        {
            bool done = false;
            do
            {
                Menu();
            } while (!done);
        }

        private static void Menu()
        {
            Console.WriteLine("-----");
            printer.PrintCyan("Menu: ");
            Console.WriteLine("-----");
            printer.PrintWhiteNewLine("[1] Add a new customer");
            printer.PrintWhiteNewLine("[2] Get data about a specific customer");
            printer.PrintWhiteNewLine("[3] Update data about an existing customer");
            printer.PrintWhiteNewLine("[4] Delete customer");
            printer.PrintWhiteNewLine("[5] Display data of all customers");
            printer.PrintWhiteNewLine("[6] Search");
            printer.PrintWhiteNewLine("[7] Quit");

            try
            {
                int choice = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        AddNewCustomer();
                        break;
                    case 2:
                        int customerId = RetrieveCustomerDataViaId();
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
                        SpecificSearch();
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                printer.PrintRed("Choose one of the alternatives in the menu!");
            }

        }

        private static void SpecificSearch()
        {
            printer.PrintWhite("Search by FirstName, LastName, Email or Phone: ");
            string choiceCol = Console.ReadLine();
            printer.PrintWhite("Search word: ");
            string searchWord = Console.ReadLine();


            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand($"SELECT  * FROM CustomerFile WHERE {choiceCol} = @SearchWord", con))
                {
                    command.Parameters.Add(new SqlParameter("SearchWord", searchWord));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        PrettyDisplayHeader();
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

        private static void PrettyDisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Id".PadRight(5));
            Console.Write("FirstName".PadRight(15));
            Console.Write("LastName".PadRight(15));
            Console.Write("Email".PadRight(30));
            Console.WriteLine("Phone".PadRight(30));
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.ResetColor();
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
            printer.PrintWhite("Delete customer with id: ");
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
            printer.PrintCyan("Update customer data".ToUpper());
            int customerId = RetrieveCustomerDataViaId();

            string connectionString = "Server = (localdb)\\mssqllocaldb; Database = CustomerFiledb";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                Console.WriteLine();
                printer.PrintWhite("Choose which column to update (FirstName, LastName, Email, Phone): ");
                string updateCol = Console.ReadLine();
                printer.PrintWhite("Replace with: ");
                string newData = Console.ReadLine();

                using (SqlCommand command = new SqlCommand($"UPDATE CustomerFile SET {updateCol} = @newData WHERE CustomerId = @CustomerId ", con))
                {
                    command.Parameters.Add(new SqlParameter("CustomerId", customerId));
                    command.Parameters.Add(new SqlParameter("newData", newData));
                    command.ExecuteNonQuery();
                }
            }
        }

        private static int RetrieveCustomerDataViaId()
        {
            printer.PrintWhite("Customer id: ");
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
                        PrettyDisplayHeader();
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
            return customerId;
        }

        private static void AddNewCustomer()
        {
            printer.PrintCyan("Add a new customer".ToUpper());

            string firstName = "";
            string lastName = "";
            string email = "";
            string phone = "";
            bool okFirstName = false;
            bool okLastName = false;
            bool okEmail = false;
            bool okPhone = false;

            do
            {
                printer.PrintWhite("First name: ");
                firstName = Console.ReadLine();
                okFirstName = ValidateName(firstName);
            } while (!okFirstName);

            do
            {
            printer.PrintWhite("Last name: ");
            lastName = Console.ReadLine();
            okLastName = ValidateName(lastName);
            } while (!okLastName);

            do
            {
            printer.PrintWhite("Email: ");
            email = Console.ReadLine();
            okEmail = ValidateEmail(email);
            } while (!okEmail);

            do
            {
            printer.PrintWhite("Phone: ");
            phone = Console.ReadLine();
            okPhone = ValidatePhone(phone);
            } while (!okPhone);

            if (okFirstName && okLastName && okEmail && okPhone)
            {
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
                       printer.PrintGreen("Successfull add of customer!");
                    }
                    catch
                    {
                        Console.WriteLine("Could not insert.");
                    }

                }

            }
        }

        private static bool ValidatePhone(string phone)
        {
            if (phone == null || phone.Length <= 2 || phone.Length > 15)
            {
                printer.PrintRed("Phone number must be between 2 and 15 characters long");
                return false;
            }
            else if (Regex.IsMatch(phone ?? "", @"^\d+-?(\d+)?$"))
            {
                return true;
            }
            else return false;
        }

        private static bool ValidateEmail(string email)
        {
            if (email == null) return false;
            else if (Regex.IsMatch(email ?? "", @"^[^\.][a-zA-Z\.0-9]*[^\.\@]\@[a-zA-Z\.]+[^\.]\.[a-zA-Z]{2,4}$"))
            {
                return true;
            }
            else return false;
        }

        private static bool ValidateName(string firstName)
        {
            if (Regex.IsMatch(firstName ?? "", @"^\s?([a-zA-Z\- ]*){2,20}$"))
            {
                return true;
            }
            else
            {
                printer.PrintRed("Not a valid name");
                return false;
            }
        }
    }
}

