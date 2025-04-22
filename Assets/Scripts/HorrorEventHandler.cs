using System.Collections;
using UnityEngine;

// Serializable class to hold both the GameObject and its duration
[System.Serializable]
public class HorrorEventData
{
    [Header("Horror Event Object")]
    public GameObject eventObject;

    [Header("Duration (seconds)")]
    [Range(0f, 5f)] public float duration = 1f;
}

public class HorrorEventHandler : MonoBehaviour
{
    [Header("Horror Event Settings")]
    [SerializeField] private int maxThreshold = 10;
    private int currentThreshold;
    private int lastEventIndex = -1;

    [Header("Horror Events List")]
    [SerializeField] private HorrorEventData[] horrorEvents;

    void Start()
    {
        currentThreshold = maxThreshold;
    }

    // Call this between questions
    public void TryTriggerHorrorEvent()
    {
        int roll = Random.Range(1, 11); // Roll between 1�10
        Debug.Log($"[Horror Roll] Rolled: {roll} | Threshold: {currentThreshold}");

        if (roll > currentThreshold)
        {
            TriggerRandomHorrorEvent();
            currentThreshold = maxThreshold; // Reset
        }
        else
        {
            currentThreshold--; // Increase chance
        }
    }

    private void TriggerRandomHorrorEvent()
    {
        if (horrorEvents.Length == 0)
        {
            Debug.LogWarning("No horror events assigned!");
            return;
        }

        int eventIndex;
        do
        {
            eventIndex = Random.Range(0, horrorEvents.Length);
        } while (eventIndex == lastEventIndex && horrorEvents.Length > 1);

        lastEventIndex = eventIndex;
        var selected = horrorEvents[eventIndex];

        Debug.Log($"[HorrorEventHandler] Triggering: {selected.eventObject.name} for {selected.duration} sec");
        selected.eventObject.SetActive(true);
        StartCoroutine(DisableAfter(selected.eventObject, selected.duration));
    }

    IEnumerator DisableAfter(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
