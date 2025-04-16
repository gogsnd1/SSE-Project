using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class LoginUIManager : MonoBehaviour
{
    // Panels
    public GameObject mainMenuPanel;
    public GameObject signUpPanel;
    public GameObject signInPanel;

    // Inputs
    public InputField signUpUsername;
    public InputField signUpPassword;
    public InputField signInUsername;
    public InputField signInPassword;

    public Text messageText;

    private string apiUrl = "https://yourserver.com/api"; // Replace with your backend URL

    void Start()
    {
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
        messageText.text = "Continuing as guest...";
        // Load next scene or enable gameplay
    }

    public void OnCreateAccountPressed()
    {
        StartCoroutine(SignUp());
    }

    public void OnLoginPressed()
    {
        StartCoroutine(SignIn());
    }

    IEnumerator SignUp()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", signUpUsername.text);
        form.AddField("password", signUpPassword.text);

        UnityWebRequest www = UnityWebRequest.Post(apiUrl + "/signup", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            messageText.text = "Account created!";
            ShowMainMenu();
        }
        else
        {
            messageText.text = "Sign up failed: " + www.error;
        }
    }

    IEnumerator SignIn()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", signInUsername.text);
        form.AddField("password", signInPassword.text);

        UnityWebRequest www = UnityWebRequest.Post(apiUrl + "/login", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            messageText.text = "Login successful!";
            // Load game scene or unlock UI
        }
        else
        {
            messageText.text = "Login failed: " + www.error;
        }
    }
}
