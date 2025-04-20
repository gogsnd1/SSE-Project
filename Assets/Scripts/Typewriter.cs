using System.Collections;
using UnityEngine;
using TMPro;

public class GlitchTypewriter : MonoBehaviour
{
    [Header("UI Setup")]
    public TextMeshProUGUI targetText;         // Drag your TMP text here
    [TextArea] public string message = "FOUND YOU :)";

    [Header("Timing")]
    public float startDelay = 2f;              // Delay before typing starts
    public float typeDelay = 0.1f;             // Time between characters

    [Header("Corruption Effects")]
    public float glitchChance = 0.2f;          // Chance each letter glitches
    public float spacingJitter = 2f;           // Random character spacing
    public float shakeAmount = 1.5f;           // Shake intensity

    private Vector3 originalPos;

    void Start()
    {
        if (targetText != null)
        {
            originalPos = targetText.rectTransform.anchoredPosition;
            StartCoroutine(TypeCorruptedText(message));
        }
    }

    IEnumerator TypeCorruptedText(string fullText)
    {
        targetText.text = "";

        // Wait before typing starts
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < fullText.Length; i++)
        {
            char realChar = fullText[i];

            if (Random.value < glitchChance)
            {
                char glitchChar = (char)Random.Range(33, 126);
                targetText.text += glitchChar;

                targetText.characterSpacing = Random.Range(-spacingJitter, spacingJitter);
                Shake();

                yield return new WaitForSeconds(typeDelay / 2f);

                targetText.text = targetText.text.Substring(0, targetText.text.Length - 1) + realChar;
            }
            else
            {
                targetText.text += realChar;
            }

            Shake();
            targetText.alpha = Random.Range(0.8f, 1f);

            yield return new WaitForSeconds(typeDelay);
        }

        targetText.characterSpacing = 0f;
        targetText.alpha = 1f;
        targetText.rectTransform.anchoredPosition = originalPos;
    }

    void Shake()
    {
        float offsetX = Random.Range(-shakeAmount, shakeAmount);
        float offsetY = Random.Range(-shakeAmount, shakeAmount);
        targetText.rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }
}
