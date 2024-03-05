using System.Linq;
using System.Text.RegularExpressions;


public class InvalidContactDetailsException : Exception
{
    public InvalidContactDetailsException(string message) : base(message)
    {
    }
}

public class Contact
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public Contact(string firstName, string lastName, string address, string city, string state, string zip, string phoneNumber, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        City = city;
        State = state;
        Zip = zip;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}

public class AddressBook
{
    private HashSet<Contact> contacts;

    //for file-handling
    private string filePath;

    public AddressBook(string filePath)
    {
        this.filePath = filePath;
        contacts = new HashSet<Contact>();
    }

    public void AddContact(Contact contact)
    {

        contacts.Add(contact);
        WriteToFile(contact); // Write contact to file
    }

    public bool Contains(string firstName, string lastName)
    {
        return contacts.Any(c => c.FirstName == firstName && c.LastName == lastName);
    }

    public void EditContact(string firstName, string lastName, Contact updatedContact)
    {
        Contact existingContact = FindContact(firstName, lastName);

        if (existingContact != null)
        {
            existingContact.FirstName = updatedContact.FirstName;
            existingContact.LastName = updatedContact.LastName;
            existingContact.Address = updatedContact.Address;
            existingContact.City = updatedContact.City;
            existingContact.State = updatedContact.State;
            existingContact.Zip = updatedContact.Zip;
            existingContact.PhoneNumber = updatedContact.PhoneNumber;
            existingContact.Email = updatedContact.Email;
        }
        else
        {
            Console.WriteLine($"Contact not found: {firstName} {lastName}");
        }
    }

    public void DeleteContact(string firstName, string lastName)
    {
        Contact contactToDelete = FindContact(firstName, lastName);

        if (contactToDelete != null)
        {
            contacts.Remove(contactToDelete);
            Console.WriteLine($"Contact deleted: {firstName} {lastName}");
        }
        else
        {
            Console.WriteLine($"Contact not found: {firstName} {lastName}");
        }
    }

    public void DisplayContacts()
    {
        foreach (var contact in contacts)
        {
            Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}, Phone: {contact.PhoneNumber}, Email: {contact.Email}, Address: {contact.Address}, City: {contact.City}, State: {contact.State}");
        }
    }
    public IEnumerable<Contact> GetContactsByCity(string city)
    {
        return contacts.Where(c => c.City.Equals(city, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Contact> GetContactsByState(string state)
    {
        return contacts.Where(c => c.State.Equals(state, StringComparison.OrdinalIgnoreCase));
    }

    private Contact FindContact(string firstName, string lastName)
    {
        return contacts.FirstOrDefault(c => c.FirstName == firstName && c.LastName == lastName);
    }

    private void WriteToFile(Contact contact)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"First Name: {contact.FirstName}");
            writer.WriteLine($"Last Name: {contact.LastName}");
            writer.WriteLine($"Address: {contact.Address}");
            writer.WriteLine($"City: {contact.City}");
            writer.WriteLine($"State: {contact.State}");
            writer.WriteLine($"Zip: {contact.Zip}");
            writer.WriteLine($"Phone Number: {contact.PhoneNumber}");
            writer.WriteLine($"Email: {contact.Email}");
            writer.WriteLine(new string('-', 50)); // Separator between contacts
        }

    }

    public void ReadFromFile()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Address book file does not exist.");
            return;
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }



    public class Program
    {
        public static void Main()
        {
            string directoryPath = @"D:\C#(LYTX)\ForTestingApplications";
            string filePath = Path.Combine(directoryPath, "AddressBook.txt");
            /*string filePath = Path.Combine(directoryPath, "AddressBook.csv");
            string filePath = Path.Combine(directoryPath, "AddressBook.xls");
            string filePath = Path.Combine(directoryPath, "AddressBook.pdf");*/

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            AddressBook myAddressBook = new AddressBook(filePath);
            bool exitProgram = false;

            do
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Display Contacts");
                Console.WriteLine("3. View Contacts by City");
                Console.WriteLine("4. View Contacts by State");
                Console.WriteLine("5. Edit Contact");
                Console.WriteLine("6. Delete Contact");
                Console.WriteLine("7. Read AddressBook.txt");
/*                Console.WriteLine("7. Read AddressBook.csv");
                Console.WriteLine("7. Read AddressBook.pdf");
                Console.Writ
eLine("7. Read AddressBook.xls");*/
                Console.WriteLine("8. Exit");

                Console.Write("Enter your choice (1-7): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            AddContact(myAddressBook);
                        }
                        catch (InvalidContactDetailsException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        break;

                    case "2":
                        myAddressBook.DisplayContacts();
                        break;

                    case "3":
                        ViewContactsByCity(myAddressBook);
                        break;

                    case "4":
                        ViewContactsByState(myAddressBook);
                        break;

                    case "5":
                        EditContact(myAddressBook);
                        break;

                    case "6":
                        DeleteContact(myAddressBook);
                        break;

                    case "7":
                        myAddressBook.ReadFromFile();
                        break;
                    case "8":
                        exitProgram = true;
                        Console.WriteLine("Exiting the program.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 8.");
                        break;
                }

            } while (!exitProgram);
        }

        // with if condition and throwing exception for the user for invalid inputs
        /*    static void AddContact(AddressBook addressBook)
            {
                Console.WriteLine("Enter details for the new Contact:");

                Console.Write("First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Address: ");
                string address = Console.ReadLine();

                Console.Write("City: ");
                string city = Console.ReadLine();

                Console.Write("State: ");
                string state = Console.ReadLine();

                Console.Write("Zip: ");
                string zip = Console.ReadLine();

                Console.Write("Phone Number: ");
                string phoneNumber = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Contact newContact = new Contact(firstName, lastName, address, city, state, zip, phoneNumber, email);

                if (!Regex.IsMatch(firstName, @"^[A-Z][a-z]+$"))
                {
                    throw new InvalidContactDetailsException("Invalid first name. Please enter a valid first name starting with a capital letter.");
                }
                if (!Regex.IsMatch(zip, "[0-9]{5}"))
                {
                    throw new InvalidContactDetailsException("Invalid zip code. Please enter a 5-digit number.");
                }
                if (!Regex.IsMatch(phoneNumber, "[5-8]{1}[0-9]{9}"))
                {
                    throw new InvalidContactDetailsException("Invalid phone number. Please enter a 10-digit number starting with 5-8.");
                } 
                if (!Regex.IsMatch(email, "[a-zA-Z0-9]+@[gmail.com,yahoo.com]"))
                {
                    throw new InvalidContactDetailsException("Invalid email address. Please enter a valid email address.");
                }



                if (addressBook.Contains(firstName,lastName))
                {
                    Console.WriteLine("Name already exists!!");
                }
                else
                {
                    addressBook.AddContact(newContact);
                    Console.WriteLine("Contact added successfully.");
                }
            }*/

        static void AddContact(AddressBook addressBook)
        {
            Console.WriteLine("Enter details for the new Contact:");

            string firstName;
            do
            {
                Console.Write("First Name: ");
                firstName = Console.ReadLine();
            } while (!IsValidFirstName(firstName));

            string lastName;
            do
            {
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();
            } while (!IsValidLastName(lastName));

            Console.Write("Address: ");
            string address = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            Console.Write("State: ");
            string state = Console.ReadLine();

            string zip;
            do
            {
                Console.Write("Zip: ");
                zip = Console.ReadLine();
            } while (!IsValidZip(zip));

            string phoneNumber;
            do
            {
                Console.Write("Phone Number: ");
                phoneNumber = Console.ReadLine();
            } while (!IsValidPhoneNumber(phoneNumber));

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
            } while (!IsValidEmail(email));

            Contact newContact = new Contact(firstName, lastName, address, city, state, zip, phoneNumber, email);
           

            if (addressBook.Contains(firstName, lastName))
            {
                Console.WriteLine("Name already exists!!");
            }
            else
            {
                addressBook.AddContact(newContact);
                Console.WriteLine("Contact added successfully.");
            }
           
        }
        static bool IsValidFirstName(string firstName)
        {
            if (!Regex.IsMatch(firstName, @"^[A-Z][a-z]+$"))
            {
                Console.WriteLine("Invalid first name. Please enter a valid first name starting with a capital letter.");
                return false;
            }
            return true;
        }

        static bool IsValidLastName(string lastName)
        {
            if (!Regex.IsMatch(lastName, @"^[A-Z][a-z]+$"))
            {
                Console.WriteLine("Invalid last name. Please enter a valid last name starting with a capital letter.");
                return false;
            }
            return true;
        }

        static bool IsValidZip(string zip)
        {
            if (!Regex.IsMatch(zip, @"^\d{5}$"))
            {
                Console.WriteLine("Invalid zip code. Please enter a 5-digit number.");
                return false;
            }
            return true;
        }

        static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!Regex.IsMatch(phoneNumber, @"^[5-8]\d{9}$"))
            {
                Console.WriteLine("Invalid phone number. Please enter a 10-digit number starting with 5-8.");
                return false;
            }
            return true;
        }

        static bool IsValidEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@gmail\.com$"))
            {
                Console.WriteLine("Invalid email address. Please enter a valid email address.");
                return false;
            }
            return true;
        }


        static void EditContact(AddressBook addressBook)
        {
            Console.WriteLine("Enter details for editing a Contact:");

            Console.Write("Enter First Name of the contact to edit: ");
            string editFirstName = Console.ReadLine();

            Console.Write("Enter Last Name of the contact to edit: ");
            string editLastName = Console.ReadLine();

            Console.WriteLine("Enter updated details:");

            Console.Write("First Name: ");
            string updatedFirstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string updatedLastName = Console.ReadLine();

            Console.Write("Address: ");
            string updatedAddress = Console.ReadLine();

            Console.Write("City: ");
            string updatedCity = Console.ReadLine();

            Console.Write("State: ");
            string updatedState = Console.ReadLine();

            Console.Write("Zip: ");
            string updatedZip = Console.ReadLine();

            Console.Write("Phone Number: ");
            string updatedPhoneNumber = Console.ReadLine();

            Console.Write("Email: ");
            string updatedEmail = Console.ReadLine();

            Contact updatedContact = new Contact(updatedFirstName, updatedLastName, updatedAddress, updatedCity, updatedState, updatedZip, updatedPhoneNumber, updatedEmail);
            addressBook.EditContact(editFirstName, editLastName, updatedContact);
        }

        static void DeleteContact(AddressBook addressBook)
        {
            Console.WriteLine("Enter details for deleting a Contact:");

            Console.Write("Enter First Name of the contact to delete: ");
            string deleteFirstName = Console.ReadLine();

            Console.Write("Enter Last Name of the contact to delete: ");
            string deleteLastName = Console.ReadLine();

            addressBook.DeleteContact(deleteFirstName, deleteLastName);
        }
        static void ViewContactsByCity(AddressBook addressBook)
        {
            Console.Write("Enter city name to view contacts: ");
            string city = Console.ReadLine();

            var contacts = addressBook.GetContactsByCity(city);
            if (contacts.Any())
            {
                Console.WriteLine($"Contacts in {city}:");
                foreach (var contact in contacts)
                {
                    Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}, Phone: {contact.PhoneNumber}, Email: {contact.Email}");
                }
            }
            else
            {
                Console.WriteLine($"No contacts found in {city}.");
            }
        }

        static void ViewContactsByState(AddressBook addressBook)
        {
            Console.Write("Enter state name to view contacts: ");
            string state = Console.ReadLine();

            var contacts = addressBook.GetContactsByState(state);
            if (contacts.Any())
            {
                Console.WriteLine($"Contacts in {state}:");
                foreach (var contact in contacts)
                {
                    Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}, Phone: {contact.PhoneNumber}, Email: {contact.Email}");
                }
            }
            else
            {
                Console.WriteLine($"No contacts found in {state}.");
            }
        }


    }
}
