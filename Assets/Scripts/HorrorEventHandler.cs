using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HorrorEventData
{
    [Header("Horror Event Object")]
    public GameObject eventObject;

    [Header("Duration (seconds)")]
    [Range(0f, 20f)]
    public float duration = 1f;
}

public class HorrorEventHandler : MonoBehaviour
{
    [Header("Horror Event Settings")]
    [SerializeField] private int maxThreshold = 10;
    private int currentThreshold;
    private List<int> unusedIndices = new List<int>();
    private int lastEventIndex = -1;

    [Header("Horror Events List")]
    [SerializeField] private HorrorEventData[] horrorEvents;

    void Start()
    {
        currentThreshold = maxThreshold;
        FillUnusedIndexList();
    }

    // Call this between questions to attempt a horror event
    public void TryTriggerHorrorEvent()
    {
        int roll = Random.Range(1, 11); // Roll from 1 to 10
        Debug.Log($"[Horror Roll] Rolled: {roll} | Threshold: {currentThreshold}");

        if (roll > currentThreshold)
        {
            TriggerRandomHorrorEvent();
            currentThreshold = maxThreshold;
        }
        else
        {
            currentThreshold--;
        }
    }

    // Call this when a player answers incorrectly to raise the odds
    public void IncreaseChanceOnFailure()
    {
        currentThreshold = Mathf.Max(currentThreshold - 2, 0);
        Debug.Log($"[HorrorEventHandler] Increased chance! New threshold: {currentThreshold}");
    }

    private void TriggerRandomHorrorEvent()
    {
        if (horrorEvents.Length == 0)
        {
            Debug.LogWarning("No horror events assigned!");
            return;
        }

        // Reset if we've gone through all events
        if (unusedIndices.Count == 0)
        {
            FillUnusedIndexList();
        }

        // Randomize from unused indices
        int randomListIndex = Random.Range(0, unusedIndices.Count);
        int eventIndex = unusedIndices[randomListIndex];
        unusedIndices.RemoveAt(randomListIndex);

        lastEventIndex = eventIndex;

        var selected = horrorEvents[eventIndex];

        Debug.Log($"[HorrorEventHandler] Triggering: {selected.eventObject.name} for {selected.duration} sec");
        selected.eventObject.SetActive(true);
        StartCoroutine(DisableAfter(selected.eventObject, selected.duration));
    }

    private void FillUnusedIndexList()
    {
        unusedIndices.Clear();
        for (int i = 0; i < horrorEvents.Length; i++)
        {
            unusedIndices.Add(i);
        }
    }

    IEnumerator DisableAfter(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
