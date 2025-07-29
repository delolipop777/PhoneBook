using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PhoneBook.Models;

namespace PhoneBook.Brokers
{
    public class FileBroker
    {
        private readonly string filePath = "Data/contacts.json";

        public List<Contact> ReadContacts()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<Contact>();
                }

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
            catch
            {
                return new List<Contact>();
            }
        }

        public void WriteContacts(List<Contact> contacts)
        {
            string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}