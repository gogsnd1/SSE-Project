using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HorrorEventData
{
    public GameObject eventObject;
    [Range(0f, 10f)] public float duration = 1f;
}

public class HorrorEventHandler : MonoBehaviour
{
    [Header("Threshold Settings")]
    [SerializeField] private int maxThreshold = 10;
    private int currentThreshold;

    [Header("Horror Events List")]
    [SerializeField] private HorrorEventData[] horrorEvents;

    private List<int> unusedEventIndices = new List<int>();

    void Start()
    {
        currentThreshold = maxThreshold;
        ResetEventPool();
    }

    public void TryTriggerHorrorEvent()
    {
        int roll = Random.Range(1, 11);
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

    private void TriggerRandomHorrorEvent()
    {
        if (unusedEventIndices.Count == 0)
        {
            Debug.Log("All events used. Resetting event pool.");
            ResetEventPool();
        }

        int randomIndex = Random.Range(0, unusedEventIndices.Count);
        int eventIndex = unusedEventIndices[randomIndex];
        unusedEventIndices.RemoveAt(randomIndex);

        var selected = horrorEvents[eventIndex];
        Debug.Log($"[HorrorEventHandler] Triggering: {selected.eventObject.name} for {selected.duration} sec");

        selected.eventObject.SetActive(true);
        StartCoroutine(DisableAfter(selected.eventObject, selected.duration));
    }

    private void ResetEventPool()
    {
        unusedEventIndices.Clear();
        for (int i = 0; i < horrorEvents.Length; i++)
            unusedEventIndices.Add(i);
    }

    IEnumerator DisableAfter(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
