using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class Program
{
    // Simulated in-memory database (username → hashed password)
    static Dictionary<string, string> userDatabase = new Dictionary<string, string>();

    // Current active user
    static string currentUsername = "";

    static void Main()
    {
        Console.WriteLine("🧠 Welcome to the Secure Login System");

        while (true)
        {
            // Menu options
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1 - Register");
            Console.WriteLine("2 - Login");
            Console.WriteLine("3 - Play as Guest");
            Console.WriteLine("4 - Exit");

            string choice = Console.ReadLine();

            // Choose what to do based on user input
            switch (choice)
            {
                case "1": Register(); break;
                case "2": Login(); break;
                case "3": GuestLogin(); break;
                case "4": return;
                default: Console.WriteLine("❌ Invalid option."); break;
            }
        }
    }

    // 🔐 Register a new user
    static void Register()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password (min 10 characters): ");
        string password = Console.ReadLine();

        // Password strength check
        if (password.Length < 10)
        {
            Console.WriteLine("❌ Password too short.");
            return;
        }

        // Prevent duplicate usernames
        if (userDatabase.ContainsKey(username))
        {
            Console.WriteLine("❌ Username already exists.");
            return;
        }

        // Hash the password and store it
        string hashed = HashPassword(password);
        userDatabase[username] = hashed;
        Console.WriteLine($"[DEBUG] Hashed password for {username}: {hashed}");


        Console.WriteLine($"✅ Registered {username} successfully.");
    }

    // 🔑 Login a user
    static void Login()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Hash the input password
        string hashed = HashPassword(password);

        Console.WriteLine($"[DEBUG] Hashed password entered: {hashed}");

         // 🔍 Show stored hash (if user exists)
        if (userDatabase.ContainsKey(username))
        {
            Console.WriteLine($"[DEBUG] Stored hash for {username}: {userDatabase[username]}");
        }

        // Check if credentials match
        if (userDatabase.ContainsKey(username) && userDatabase[username] == hashed)
        {
            currentUsername = username;
            Console.WriteLine($"✅ Welcome back, {currentUsername}!");
        }
        else
        {
            Console.WriteLine("❌ Invalid login.");
        }
    }

    // 👤 Guest login with random username
    static void GuestLogin()
    {
        Random rnd = new Random();
        currentUsername = "Guest_" + rnd.Next(1000, 9999);
        Console.WriteLine($"👋 You're logged in as {currentUsername}.");
    }

    // 🔐 SHA-256 password hashing function
    static string HashPassword(string password)
    {
        // Convert the password into byte array
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);

        // Use SHA-256 hash algorithm
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // Convert bytes to hex string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2")); // "x2" = 2-digit hex
            return sb.ToString();
        }
    }
}
