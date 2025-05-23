using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class RadioButtonSystem : MonoBehaviour
{
    
    private ToggleGroup toggleGroup;

    [Header("Horror Integration")]
    public HorrorEventHandler horrorHandler;  // Assign this in the Inspector


    [Header("Correct Answer Settings")]
    public Toggle correctToggle;  // Drag the correct toggle here

    [Header("Audio Feedback")]
    public AudioSource audioSource;  // Assign an AudioSource component
    public AudioClip correctAudio;   // Drag the correct sound
    public AudioClip incorrectAudio; // Drag the incorrect sound

    [Header("Submit Button")]
    public Button submitButton;   // Drag your Submit button (Next arrow) here

       void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        submitButton.gameObject.SetActive(false);  // Hide submit at start

        // Deselect all toggles and add listeners
        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;  // Deselect toggle
            toggle.onValueChanged.AddListener(delegate { OnToggleSelected(); });
        }
    }

    void OnToggleSelected()
    {
        // Check if any toggle is active
        if (toggleGroup.AnyTogglesOn())
        {
            submitButton.gameObject.SetActive(true);  // Show submit
        }
        else
        {
            submitButton.gameObject.SetActive(false); // Hide if nothing selected
        }
    }

    public void Submit()
    {
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedToggle != null)
        {
            TMP_Text answerText = selectedToggle.GetComponentInChildren<TMP_Text>();

            if (answerText != null)
            {
                Debug.Log(selectedToggle.name + " _ " + answerText.text);

                // Check correctness
                if (selectedToggle == correctToggle)
                {
                    Debug.Log("Correct Answer!");
                    audioSource.PlayOneShot(correctAudio);
                    ScoreTracking.Instance.AddScore();
                }
                else
                {
                    Debug.Log("Incorrect Answer.");
                    audioSource.PlayOneShot(incorrectAudio);
                    if (horrorHandler != null)
                {
                // Increase horror chance on incorrect answer
                horrorHandler.IncreaseChanceOnFailure();
                }
                }

                // Lock submission
                submitButton.interactable = false;

                // Optionally disable toggles after submission
                foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
                {
                    toggle.interactable = false;
                }
            }
            else
            {
                Debug.LogWarning("TMP_Text component not found in selected toggle.");
            }
        }
        else
        {
            Debug.LogWarning("No toggle selected.");
        }
    }
}
