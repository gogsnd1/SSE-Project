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
        if (scoreUploader != null)
        {
            scoreUploader.UploadFinalScore();  // Trigger score upload
        }

        yield return new WaitForSeconds(1.0f);  // Wait for 1 second (adjust as needed)

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
