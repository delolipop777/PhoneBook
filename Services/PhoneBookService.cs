using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneBook.Brokers;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    public class PhoneBookService
    {
        private readonly FileBroker fileBroker;
        private readonly LoggingBroker loggingBroker;

        public PhoneBookService()
        {
            this.fileBroker = new FileBroker();
            this.loggingBroker = new LoggingBroker();
        }

        public void AddContact()
        {
            try
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine()!;

                Console.Write("Enter Phone Number: ");
                string phoneNumber = Console.ReadLine()!;

                var contact = new Contact
                {
                    Name = name,
                    PhoneNumber = phoneNumber
                };

                var contacts = fileBroker.ReadContacts();
                contacts.Add(contact);
                fileBroker.WriteContacts(contacts);

                loggingBroker.LogInfo("Contact added succesfully");
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Failed to add contact: {ex.Message}");
            }
        }

        public void ViewAllContacts()
        {
            try
            {
                var contacts = fileBroker.ReadContacts();

                if (contacts.Count == 0)
                {
                    loggingBroker.LogInfo("No contacts found");
                    return;
                }

                foreach (var contact in contacts)
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine(contact);
                }
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Failed to load contacts: {ex.Message}");
            }
        }

        public void SearchContactByName()
        {
            try
            {
                Console.Write($"Enter name to search: ");
                string name = Console.ReadLine()!;

                var contacts = fileBroker.ReadContacts();
                var found = contacts.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (found.Count == 0)
                {
                    loggingBroker.LogInfo("No contacts found");
                    return;
                }

                foreach (var contact in found)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine(contact);
                }
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Failed to search contacts: {ex.Message}");
            }
        }

        public void DeleteContact()
        {
            try
            {
                Console.Write("Enter name of contact to delete: ");
                string name = Console.ReadLine()!;

                var contacts = fileBroker.ReadContacts();
                var matches = contacts
                    .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matches.Count == 0)
                {
                    loggingBroker.LogInfo("No contacts found with that name.");
                    return;
                }
                Contact toRemove;
                if (matches.Count == 1)
                {
                    toRemove = matches[0];
                }
                else
                {
                    Console.WriteLine($"Matching contacts:");
                    for (int i = 0; i < matches.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {matches[i]}");
                    }
                contactAsk:
                    Console.Write("Enter the number of the contact to delete: ");

                    if (int.TryParse(Console.ReadLine()!, out int choice) && choice >= 1 && choice <= matches.Count)
                    {
                        toRemove = matches[choice - 1];
                    }
                    else
                    {
                        loggingBroker.LogWarning("Invalid choice. No contact deleted.");
                        goto contactAsk;
                    }
                }
                contacts.Remove(toRemove);
                fileBroker.WriteContacts(contacts);
                loggingBroker.LogInfo("Contact deleted succesfully!");
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Failed to delete contact: {ex.Message}");
            }
        }

        public void EditContact()
        {
            try
            {
                Console.Write("Enter name of contact to edit: ");
                string name = Console.ReadLine()!;

                var contacts = fileBroker.ReadContacts();
                var matches = contacts
                    .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matches.Count == 0)
                {
                    loggingBroker.LogInfo("No contacts found with that name.");
                    return;
                }
                Contact toEdit;
                if (matches.Count == 1)
                {
                    toEdit = matches[0];
                }
                else
                {
                    Console.WriteLine($"Matching contacts:");
                    for (int i = 0; i < matches.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {matches[i]}");
                    }
                contactAsk:
                    Console.Write("Enter the number of the contact to edit: ");

                    int choiceTemp;
                    if (int.TryParse(Console.ReadLine()!, out choiceTemp) && choiceTemp >= 1 && choiceTemp <= matches.Count)
                    {
                        toEdit = matches[choiceTemp - 1];
                    }
                    else
                    {
                        loggingBroker.LogWarning("Invalid choice.");
                        goto contactAsk;
                    }
                }

                Console.WriteLine("What do you want to edit?");
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Phone Number");
                Console.WriteLine("3. Both");
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine()!, out int choice) && choice >= 1 && choice <= 3)
                {
                    if (choice == 1)
                    {
                        Console.Write("Enter new name: ");
                        toEdit.Name = Console.ReadLine()!;
                    }
                    else if (choice == 2)
                    {
                        Console.Write("Enter new phone number: ");
                        toEdit.PhoneNumber = Console.ReadLine()!;
                    }
                    else
                    {
                        Console.Write("Enter new name: ");
                        toEdit.Name = Console.ReadLine()!;
                        Console.Write("Enter new phone number: ");
                        toEdit.PhoneNumber = Console.ReadLine()!;
                    }
                }
                else
                {
                    loggingBroker.LogWarning("Invalid choice.");
                }

                fileBroker.WriteContacts(contacts);
                loggingBroker.LogInfo("Contact edited successfully!");

            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Failed to edit contact: {ex.Message}");
            }
        }
    }
}