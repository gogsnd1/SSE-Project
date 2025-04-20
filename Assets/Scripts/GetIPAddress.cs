using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CorruptedIPRevealWithAudio : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI foundYouText;
    public TextMeshProUGUI ipText;

    [Header("Corruption Settings")]
    public float typeDelay = 0.1f;
    public float glitchChance = 0.2f;
    public float spacingJitter = 2f;
    public float shakeAmount = 1.5f;

    [Header("Audio")]
    public AudioSource typingAudio;     // Drag your AudioSource here

    private string currentIP = "Unknown";
    private Vector3 originalIPPos;
    private Vector3 originalFoundPos;

    void Start()
    {
        if (foundYouText != null)
            originalFoundPos = foundYouText.rectTransform.anchoredPosition;

        if (ipText != null)
            originalIPPos = ipText.rectTransform.anchoredPosition;

        StartCoroutine(GetPublicIP());
    }

    IEnumerator GetPublicIP()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://api.ipify.org");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Failed to get real IP. Using fallback.");
            currentIP = GenerateFakeIP();
        }
        else
        {
            currentIP = request.downloadHandler.text;
        }

        // Start audio
        if (typingAudio != null)
        {
            typingAudio.Play();
        }

        // Display corrupted "FOUND YOU :)"
        if (foundYouText != null)
            yield return StartCoroutine(TypeCorruptedText(foundYouText, "FOUND YOU :)", originalFoundPos));

        yield return new WaitForSeconds(0.4f);

        // Display corrupted IP line
        if (ipText != null)
            yield return StartCoroutine(TypeCorruptedText(ipText, $"IP: {currentIP}", originalIPPos));

        // Stop audio
        if (typingAudio != null)
        {
            typingAudio.Stop();
        }
    }

    IEnumerator TypeCorruptedText(TextMeshProUGUI targetText, string fullText, Vector3 originalPos)
    {
        targetText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            char realChar = fullText[i];

            if (Random.value < glitchChance)
            {
                char glitchChar = (char)Random.Range(33, 126);
                targetText.text += glitchChar;

                targetText.characterSpacing = Random.Range(-spacingJitter, spacingJitter);
                Shake(targetText, originalPos);

                yield return new WaitForSeconds(typeDelay / 2f);

                targetText.text = targetText.text.Substring(0, targetText.text.Length - 1) + realChar;
            }
            else
            {
                targetText.text += realChar;
            }

            Shake(targetText, originalPos);
            targetText.alpha = Random.Range(0.8f, 1f);

            yield return new WaitForSeconds(typeDelay);
        }

        targetText.characterSpacing = 0f;
        targetText.alpha = 1f;
        targetText.rectTransform.anchoredPosition = originalPos;
    }

    void Shake(TextMeshProUGUI target, Vector3 originalPos)
    {
        float offsetX = Random.Range(-shakeAmount, shakeAmount);
        float offsetY = Random.Range(-shakeAmount, shakeAmount);
        target.rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }

    string GenerateFakeIP()
    {
        return "192.168." + Random.Range(0, 255) + "." + Random.Range(1, 254);
    }
}
