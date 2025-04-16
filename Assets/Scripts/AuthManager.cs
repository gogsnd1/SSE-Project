using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AuthManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public Text messageText;

    private string apiUrl = "https://yourserver.com/api"; // Replace with your backend URL

    public void OnSignUpButton()
    {
        StartCoroutine(SignUp());
    }

    public void OnLoginButton()
    {
        StartCoroutine(Login());
    }

    IEnumerator SignUp()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("email", emailInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(apiUrl + "/signup", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            messageText.text = "Sign up successful!";
        }
        else
        {
            messageText.text = "Sign up failed: " + www.error;
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(apiUrl + "/login", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            messageText.text = "Login successful!";
            // Optionally store token/session data here
        }
        else
        {
            messageText.text = "Login failed: " + www.error;
        }
    }
}
