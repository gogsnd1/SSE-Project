/*using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SQLite4Unity3d; // NEW: SQLite4Unity3d namespace

public class LoginUIManager : MonoBehaviour
{
    // Panels
    public GameObject mainMenuPanel;
    public GameObject signUpPanel;
    public GameObject signInPanel;

    // Inputs for sign-up and sign-in
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
        db.CreateTable<User>(); // Ensure table exists

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
        // TODO: Store guest name or transition to gameplay
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

        var existingUser = db.Table<User>().Where(u => u.username == username).FirstOrDefault();
        if (existingUser != null)
        {
            ShowMessage("Username already exists.");
            return;
        }

        db.Insert(new User { username = username, password = hashed });

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

        var user = db.Table<User>().Where(u => u.username == username && u.password == hashed).FirstOrDefault();

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

    // Class that maps to 'users' table in your .sqlite file
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
*/