using System.Collections;
using UnityEngine;


public class HorrorEventHandler : MonoBehaviour
{
    [Header("Horror Event Settings")]
    [SerializeField] private int maxThreshold = 10;
    private int currentThreshold;
    private int lastEventIndex = -1;

    [Header("Horror Events Array")]
    public GameObject[] Events; // Drag & drop the 15 Event GameObjects in Inspector

    void Start()
    {
        currentThreshold = maxThreshold; // Reset threshold at start

    }

    // Call this between questions
    public void TryTriggerHorrorEvent()
    {
        int roll = Random.Range(1, 11); // Random roll 1–10
        Debug.Log($"[Horror Roll] Rolled: {roll} | Threshold: {currentThreshold}");

        if (roll > currentThreshold)
        {
            TriggerRandomHorrorEvent();
            currentThreshold = maxThreshold; // Reset threshold after event
        }
        else
        {
            currentThreshold--; // Make event more likely next time
        }
    }

    private void TriggerRandomHorrorEvent()
    {
        if (Events.Length == 0)
        {
            Debug.LogWarning("No horror events assigned!");
            return;
        }

        int eventIndex;

        // Pick a different event than last time
        do
        {
            eventIndex = Random.Range(0, Events.Length);
        } while (eventIndex == lastEventIndex && Events.Length > 1);

        lastEventIndex = eventIndex;

        Debug.Log($"Triggering Horror Event #{eventIndex}");

        GameObject selectedEvent = Events[eventIndex];
        selectedEvent.SetActive(true); // Activate the GameObject

        // Optional: Auto-disable after a delay
        StartCoroutine(DisableAfter(selectedEvent, 4f));
    }

    // Reusable coroutine to disable an object after a few seconds
    IEnumerator DisableAfter(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}