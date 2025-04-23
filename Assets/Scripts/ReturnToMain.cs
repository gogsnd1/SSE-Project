using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    public ScoreUploader scoreUploader;  // Reference to ScoreUploader

    public void returnMain()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(DelayedReturn());
    }

    private System.Collections.IEnumerator DelayedReturn()
    {
        // Upload the score before transitioning
        if (scoreUploader != null)
        {
            int userId = LoginUIManager.LoggedInUserId;  // Assuming you have this setup
            int finalScore = ScoreTracking.Instance != null ? ScoreTracking.Instance.GetScore() : 0;

            scoreUploader.UploadFinalScore();  // Upload score
        }

        yield return new WaitForSeconds(1.0f);  // Wait for 1 second to ensure upload

        // Return to Main Menu (assuming previous scene is main)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
