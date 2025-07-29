using System;
using PhoneBook.Services;

namespace PhoneBook
{
    class Program
    {
        static void Main(string[] args)
        {
            var phoneBookService = new PhoneBookService();
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("====== PHONE BOOK ======");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. View All Contacts");
                Console.WriteLine("3. Search Contact");
                Console.WriteLine("4. Delete Contact");
                Console.WriteLine("5. Edit Contact");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option (1-6): ");

                string input = Console.ReadLine()!;
                Console.Clear();

                switch (input)
                {
                    case "1":
                        phoneBookService.AddContact();
                        break;
                    case "2":
                        phoneBookService.ViewAllContacts();
                        break;
                    case "3":
                        phoneBookService.SearchContactByName();
                        break;
                    case "4":
                        phoneBookService.DeleteContact();
                        break;
                    case "5":
                        phoneBookService.EditContact();
                        break;
                    case "6":
                        isRunning = false;
                        Console.WriteLine("Exiting the application...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (isRunning)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }
    }
}
