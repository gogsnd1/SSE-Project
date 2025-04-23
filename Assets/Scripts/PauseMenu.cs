using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public LookScript lookScript;
    public AudioMixer audioMixer;
    public AudioMixerSnapshot gameplaySnapshot;
    public AudioMixerSnapshot pausedSnapshot;

    private AudioSource[] allAudioSources;

    void Start()
    {
        allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                gameResume();
            else
                gamePause();
        }
    }

    public void gameResume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;

        gameplaySnapshot.TransitionTo(0.1f);

        // Resume all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            audio.UnPause();
        }

        lookScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void gamePause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        pausedSnapshot.TransitionTo(0.1f);

        // Pause all audio sources
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying)
                audio.Pause();
        }

        lookScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
