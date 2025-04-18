using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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

    // Path to SQLite database
    private string dbPath;

    void Start()
    {
        dbPath = "URI=file:" + Path.Combine(Application.streamingAssetsPath, "gamedatabase.sqlite");
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
        // You could store guest name here for gameplay session
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

        using (IDbConnection db = new SqliteConnection(dbPath))
        {
            db.Open();
            IDbCommand checkCmd = db.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM users WHERE username = @username";
            AddParam(checkCmd, "@username", username);
            int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists > 0)
            {
                ShowMessage("Username already exists.");
                return;
            }

            IDbCommand insertCmd = db.CreateCommand();
            insertCmd.CommandText = "INSERT INTO users (username, password) VALUES (@u, @p)";
            AddParam(insertCmd, "@u", username);
            AddParam(insertCmd, "@p", hashed);
            insertCmd.ExecuteNonQuery();
        }

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

        using (IDbConnection db = new SqliteConnection(dbPath))
        {
            db.Open();
            IDbCommand cmd = db.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM users WHERE username = @u AND password = @p";
            AddParam(cmd, "@u", username);
            AddParam(cmd, "@p", hashed);

            int match = Convert.ToInt32(cmd.ExecuteScalar());
            if (match > 0)
                ShowMessage("Login successful!");
            else
                ShowMessage("Invalid login.");
        }
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

    private void AddParam(IDbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }
}
