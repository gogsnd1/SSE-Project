using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SQLite4Unity3d; // Required for SQLite4Unity3d support

public class LoginUIManager : MonoBehaviour
{
    // UI Panels
    public GameObject mainMenuPanel;
    public GameObject signUpPanel;
    public GameObject signInPanel;

    // UI Input Fields
    public InputField signUpUsername;
    public InputField signUpPassword;
    public InputField signInUsername;
    public InputField signInPassword;

    // Feedback display
    public Text messageText;

    private SQLiteConnection db;

    void Start()
    {
        string dbPath = Path.Combine(Application.streamingAssetsPath, "gamedatabase.sqlite");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        db.CreateTable<users>(); // Maps to your 'users' table

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        signUpPanel.SetActive(false);
        signInPanel.SetActive(false);
        messageText.text = "";
    }

    public void OnSignUpPressed()
    {
        mainMenuPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    public void OnSignInPressed()
    {
        mainMenuPanel.SetActive(false);
        signInPanel.SetActive(true);
    }

    public void OnGuestPressed()
    {
        string guestName = "Guest_" + UnityEngine.Random.Range(1000, 9999);
        messageText.text = "Welcome, " + guestName + "!";
    }

    public void OnCreateAccountPressed()
    {
        string username = signUpUsername.text.Trim();
        string password = signUpPassword.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Please fill all fields.");
            return;
        }

        if (password.Length < 10)
        {
            ShowMessage("Password must be at least 10 characters.");
            return;
        }

        string hashed = HashPassword(password);

        var existingUser = db.Table<users>().Where(u => u.user_username == username).FirstOrDefault();
        if (existingUser != null)
        {
            ShowMessage("Username already exists.");
            return;
        }

        db.Insert(new users
        {
            user_username = username,
            user_password = hashed,
            created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });

        ShowMessage("Account created!");
        ShowMainMenu();
    }

    public void OnLoginPressed()
    {
        string username = signInUsername.text.Trim();
        string password = signInPassword.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Enter username and password.");
            return;
        }

        string hashed = HashPassword(password);

        var user = db.Table<users>()
            .Where(u => u.user_username == username && u.user_password == hashed)
            .FirstOrDefault();

        if (user != null)
            ShowMessage("Login successful!");
        else
            ShowMessage("Invalid login.");
    }

    private void ShowMessage(string msg)
    {
        messageText.text = msg;
        Debug.Log("[LoginUIManager] " + msg);
    }

    private string HashPassword(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    // Maps directly to your 'users' table
    public class users
    {
        [PrimaryKey, AutoIncrement]
        public int user_id { get; set; }
        public string user_username { get; set; }
        public string user_password { get; set; }
        public string created_at { get; set; }
    }
}
