using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputResponder : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text questionText;      // Drag your question text
    public TMP_InputField inputField;  // Drag your TMP InputField
    public TMP_Text responseText;      // Drag your response TMP_Text
    public GameObject arrowButton;     // Drag your arrow (button or image)

    [Header("Custom Response")]
    [TextArea]
    public string customResponse = "We saw it move. You should turn around.";

    private TMP_Text inputTextComponent;
    private Vector3 inputFieldOriginalPos;

    void Start()
    {
        responseText.gameObject.SetActive(false);
        arrowButton.SetActive(false);

        inputField.onSubmit.AddListener(OnInputSubmit);
        inputField.onValueChanged.AddListener(delegate { ApplyInputJitter(); });

        inputTextComponent = inputField.textComponent;
        inputFieldOriginalPos = inputField.GetComponent<RectTransform>().localPosition;  // Store original position
    }

    void OnInputSubmit(string userInput)
    {
        if (!string.IsNullOrEmpty(userInput))
        {
            responseText.text = customResponse;
            responseText.gameObject.SetActive(true);
            arrowButton.SetActive(true);

            // Hide input and question
            inputField.gameObject.SetActive(false);
            questionText.gameObject.SetActive(false);

            // Start glitch on response text
            StartCoroutine(GlitchResponse());
        }
    }

    void ApplyInputJitter()
    {
        // Reset InputField position
        inputField.GetComponent<RectTransform>().localPosition = inputFieldOriginalPos;

        // Shake the InputField box
        Vector3 shakeOffset = new Vector3(
            Random.Range(-2f, 2f),
            Random.Range(-2f, 2f),
            0f
        );
        inputField.GetComponent<RectTransform>().localPosition += shakeOffset;

        // Jitter the input text
        inputTextComponent.ForceMeshUpdate();
        var textInfo = inputTextComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

            Vector3 jitterOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0f
            );

            vertices[vertexIndex + 0] += jitterOffset;
            vertices[vertexIndex + 1] += jitterOffset;
            vertices[vertexIndex + 2] += jitterOffset;
            vertices[vertexIndex + 3] += jitterOffset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            inputTextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    System.Collections.IEnumerator GlitchResponse()
    {
        while (true)
        {
            responseText.ForceMeshUpdate();
            var textInfo = responseText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

                Vector3 glitchOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0f
                );

                vertices[vertexIndex + 0] += glitchOffset;
                vertices[vertexIndex + 1] += glitchOffset;
                vertices[vertexIndex + 2] += glitchOffset;
                vertices[vertexIndex + 3] += glitchOffset;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                responseText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            // Flicker color and scale
            responseText.color = Random.value > 0.9f ? Color.red : Color.white;
            responseText.rectTransform.localScale = Vector3.one * Random.Range(0.95f, 1.05f);

            yield return new WaitForSeconds(0.05f);
        }
    }
}
