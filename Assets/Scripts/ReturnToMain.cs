using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    public void returnMain()
    {
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
        Cursor.visible = true;                   // Make cursor visible

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
