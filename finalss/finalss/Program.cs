using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalss
{
    class User
    {
        public string Username;
        public string Password;
        public string Role;
        public bool IsActive;
    }
    class Product
    {
        public string Name;
        public int Quantity;
        public bool IsActive;
        public DateTime CreatedDate;
        public string CreatedBy;
    }
    class Order
    {
        public string ProductName;
        public int Quantity;
        public string OrderedBy;
    }
    internal class Program
    {
        static List<User> users = new List<User>();
        static List<Product> products = new List<Product>();
        static List<Order> orders = new List<Order>();
        static void Main(string[] args)
        {
            AddDefaultUser("admin", "admin123", "Admin");
            AddDefaultUser("jayson_warehouse", "1234", "Warehouse");
            AddDefaultUser("jayson_user", "1234", "User");
            while (true)
            {
                User loggedIn = Login();
                if (loggedIn == null)
                {
                    continue;
                }
                switch (loggedIn.Role)
                {
                    case "Admin":
                        AdminMenu(loggedIn);
                        break;
                    case "Warehouse":
                        WarehouseMenu(loggedIn);
                        break;
                    default:
                        UserMenu(loggedIn);
                        break;
                }
            }
        }
        static void AddDefaultUser(string username, string password, string role)
        {
            User u = new User();
            u.Username = username;
            u.Password = password;
            u.Role = role;
            u.IsActive = true;
            users.Add(u);
        }
        static int ReadNumber()
        {
            int number;
            string input = Console.ReadLine();
            if (int.TryParse(input, out number))
            {
                return number;
            }
            return -1;
        }
        static User Login()
        {
            Console.WriteLine();
            Console.WriteLine("Please select a user role to login:");
            Console.WriteLine("1 Admin");
            Console.WriteLine("2 Warehouse");
            Console.WriteLine("3 User");
            int choice = ReadNumber();
            string role = "";
            switch (choice)
            {
                case 1:
                    role = "Admin";
                    break;
                case 2:
                    role = "Warehouse";
                    break;
                case 3:
                    role = "User";
                    break;
                default:
                    Console.WriteLine("Invalid role.");
                    return null;
            }
            Console.Write("Please enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine();
            foreach (User u in users)
            {
                if (u.Username.ToLower() == username.ToLower() && u.Password == password && u.Role == role)
                {
                    if (u.IsActive == false)
                    {
                        Console.WriteLine("This account is inactive.");
                        return null;
                    }
                    Console.WriteLine("Login successful. Welcome, " + u.Username + "!");
                    return u;
                }
            }
            Console.WriteLine("Wrong username or password.");
            return null;
        }
        static void AdminMenu(User admin)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Update Password");
                Console.WriteLine("2. Inactivate User");
                Console.WriteLine("3. Add User");
                Console.WriteLine("4. Display All Users");
                Console.WriteLine("5. Display Inactive Users");
                Console.WriteLine("6. Display Active Users");
                Console.WriteLine("7. Display Products");
                Console.WriteLine("8. Display Orders");
                Console.WriteLine("9. Logout");
                int choice = ReadNumber();
                switch (choice)
                {
                    case 1:
                        UpdatePassword(admin);
                        break;
                    case 2:
                        InactivateUser(admin);
                        break;
                    case 3:
                        AddUser();
                        break;
                    case 4:
                        DisplayUsers("all");
                        break;
                    case 5:
                        DisplayUsers("inactive");
                        break;
                    case 6:
                        DisplayUsers("active");
                        break;
                    case 7:
                        DisplayProducts();
                        break;
                    case 8:
                        DisplayOrders();
                        break;
                    case 9:
                        Console.WriteLine("Logged out.");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        static void UpdatePassword(User admin)
        {
            List<User> activeUsers = new List<User>();
            foreach (User u in users)
            {
                if (u.IsActive && u != admin)
                {
                    activeUsers.Add(u);
                }
            }
            Console.WriteLine("1. My account");
            for (int i = 0; i < activeUsers.Count; i++)
            {
                Console.WriteLine((i + 2) + ". " + activeUsers[i].Username);
            }
            int choice = ReadNumber();
            User selected = null;
            switch (choice)
            {
                case 1:
                    selected = admin;
                    break;
                default:
                    if (choice >= 2 && choice <= activeUsers.Count + 1)
                    {
                        selected = activeUsers[choice - 2];
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        return;
                    }
                    break;
            }
            Console.Write("Please enter the new password for user " + selected.Username + ": ");
            string newPassword = Console.ReadLine();
            Console.Write("Please confirm the password: ");
            string confirm = Console.ReadLine();
            if (newPassword == confirm && newPassword != "")
            {
                selected.Password = newPassword;
                Console.WriteLine("Password updated successfully.");
            }
            else
            {
                Console.WriteLine("Passwords do not match. Password not changed.");
            }
        }
        static void InactivateUser(User admin)
        {
            List<User> activeUsers = new List<User>();
            foreach (User u in users)
            {
                if (u.IsActive && u != admin)
                {
                    activeUsers.Add(u);
                }
            }
            if (activeUsers.Count == 0)
            {
                Console.WriteLine("No active users to inactivate.");
                return;
            }
            for (int i = 0; i < activeUsers.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + activeUsers[i].Username);
            }
            Console.Write("Please enter the number of the user to inactivate: ");
            int choice = ReadNumber();
            if (choice >= 1 && choice <= activeUsers.Count)
            {
                activeUsers[choice - 1].IsActive = false;
                Console.WriteLine(activeUsers[choice - 1].Username + " has been inactivated.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
        static void AddUser()
        {
            Console.Write("Please enter the new username: ");
            string username = Console.ReadLine();
            foreach (User u in users)
            {
                if (u.Username.ToLower() == username.ToLower())
                {
                    Console.WriteLine("That username already exists.");
                    return;
                }
            }
            Console.Write("Please enter the password: ");
            string password = Console.ReadLine();
            Console.WriteLine("Please select the role:");
            Console.WriteLine("1 Admin");
            Console.WriteLine("2 Warehouse");
            Console.WriteLine("3 User");
            int choice = ReadNumber();
            string role = "";
            switch (choice)
            {
                case 1:
                    role = "Admin";
                    break;
                case 2:
                    role = "Warehouse";
                    break;
                case 3:
                    role = "User";
                    break;
                default:
                    Console.WriteLine("Invalid role.");
                    return;
            }
            AddDefaultUser(username, password, role);
            Console.WriteLine("User has been successfully added.");
        }
        static void DisplayUsers(string type)
        {
            int count = 0;
            foreach (User u in users)
            {
                bool show = false;
                switch (type)
                {
                    case "all":
                        show = true;
                        break;
                    case "active":
                        if (u.IsActive) show = true;
                        break;
                    case "inactive":
                        if (u.IsActive == false) show = true;
                        break;
                }
                if (show)
                {
                    count++;
                    string status = "Inactive";
                    if (u.IsActive) status = "Active";
                    Console.WriteLine();
                    Console.WriteLine("Username: " + u.Username);
                    Console.WriteLine("Role: " + u.Role);
                    Console.WriteLine("Status: " + status);
                }
            }
            if (count == 0)
            {
                Console.WriteLine("No users found.");
            }
        }
        static void WarehouseMenu(User warehouse)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Add Products");
                Console.WriteLine("2. Display Products");
                Console.WriteLine("3. Logout");
                int choice = ReadNumber();
                switch (choice)
                {
                    case 1:
                        AddProducts(warehouse);
                        break;
                    case 2:
                        DisplayProducts();
                        break;
                    case 3:
                        Console.WriteLine("Logged out.");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        static void AddProducts(User warehouse)
        {
            Console.Write("Please enter the number of products you want to add: ");
            int howMany = ReadNumber();
            if (howMany < 1)
            {
                Console.WriteLine("Invalid number.");
                return;
            }
            for (int i = 1; i <= howMany; i++)
            {
                Product p = new Product();
                Console.Write(i + ". Please enter the name of the product : ");
                p.Name = Console.ReadLine();
                Console.Write(i + ". Please enter the quantity of the product : ");
                p.Quantity = ReadNumber();
                if (p.Quantity < 0)
                {
                    p.Quantity = 0;
                }
                p.IsActive = true;
                p.CreatedDate = DateTime.Now;
                p.CreatedBy = warehouse.Username;
                products.Add(p);
            }
            Console.WriteLine("All products have been successfully created.");
        }
        static void DisplayProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("No products yet.");
                return;
            }
            foreach (Product p in products)
            {
                string active = "No";
                if (p.IsActive) active = "Yes";
                Console.WriteLine();
                Console.WriteLine("Product Name: " + p.Name);
                Console.WriteLine("Quantity: " + p.Quantity);
                Console.WriteLine("Active : " + active);
                Console.WriteLine("Created Date: " + p.CreatedDate);
                Console.WriteLine("Created By: " + p.CreatedBy);
            }
        }
        static void UserMenu(User user)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Order");
                Console.WriteLine("2. Display Order");
                Console.WriteLine("3. Logout");
                int choice = ReadNumber();
                switch (choice)
                {
                    case 1:
                        MakeOrder(user);
                        break;
                    case 2:
                        DisplayMyOrders(user);
                        break;
                    case 3:
                        Console.WriteLine("Logged out.");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        static void MakeOrder(User user)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("There are no products to order yet.");
                return;
            }
            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine((i + 1) + ".Product Name: " + products[i].Name);
                Console.WriteLine("Quantity: " + products[i].Quantity);
                Console.WriteLine();
            }
            Console.Write("Please enter the number of the product you want to order:");
            int number = ReadNumber();
            if (number < 1 || number > products.Count)
            {
                Console.WriteLine("Invalid product number.");
                return;
            }
            Product chosen = products[number - 1];
            Console.Write("Please enter the quantity of the product you want to order: ");
            int quantity = ReadNumber();
            if (quantity < 1)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }
            if (quantity > chosen.Quantity)
            {
                Console.WriteLine("Not enough stock. Only " + chosen.Quantity + " left.");
                return;
            }
            Order o = new Order();
            o.ProductName = chosen.Name;
            o.Quantity = quantity;
            o.OrderedBy = user.Username;
            orders.Add(o);
            chosen.Quantity = chosen.Quantity - quantity;
            Console.WriteLine("Order has been successfully created.");
            Console.WriteLine();
            Console.WriteLine("Order Details:");
            Console.WriteLine("Product Name: " + o.ProductName);
            Console.WriteLine("Quantity: " + o.Quantity);
        }
        static void DisplayMyOrders(User user)
        {
            int count = 0;
            foreach (Order o in orders)
            {
                if (o.OrderedBy == user.Username)
                {
                    count++;
                    Console.WriteLine();
                    Console.WriteLine("Product Name: " + o.ProductName);
                    Console.WriteLine("Quantity: " + o.Quantity);
                }
            }
            if (count == 0)
            {
                Console.WriteLine("You have no orders yet.");
            }
        }
        static void DisplayOrders()
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders yet.");
                return;
            }
            foreach (Order o in orders)
            {
                Console.WriteLine();
                Console.WriteLine("Product Name: " + o.ProductName);
                Console.WriteLine("Quantity: " + o.Quantity);
                Console.WriteLine("Ordered By: " + o.OrderedBy);
            }
        }
    }
}