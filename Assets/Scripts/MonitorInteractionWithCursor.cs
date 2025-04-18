using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MonitorInteractionWithCursor : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject monitor;
    public Canvas monitorCanvas;
    public RectTransform customCursor;
    public float interactionDistance = 3f;
    public AudioSource audioSource;
    private bool isCursorActive = false;

    [Header("Click Sound Settings")]
    [SerializeField] AudioClip clickSound;         // Assign sound in Inspector
    [SerializeField][Range(0f, 1f)] float clickVolume = 0.5f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        customCursor.gameObject.SetActive(false);
    }

    void Update()
    {

        if ( Input.GetKeyDown(KeyCode.Space))
        {
            ToggleCustomCursor();
            //TODO: Stop sound from playing when Spacebar is pressed

        }

        if (isCursorActive)
        {
            // Play click sound with volume
            if (Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space))
            { 
                audioSource.PlayOneShot(clickSound, clickVolume);
            
            }

            UpdateCursorPosition();
        }
    }

    void ToggleCustomCursor()
    {
        isCursorActive = !isCursorActive;
        
        Cursor.visible = false;
        Cursor.lockState = (isCursorActive ? CursorLockMode.None : CursorLockMode.Locked);

        customCursor.gameObject.SetActive(isCursorActive);
    }

   void UpdateCursorPosition()
{
    Vector2 localPoint;
    RectTransform canvasRect = monitorCanvas.GetComponent<RectTransform>();

    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect, Input.mousePosition, playerCamera, out localPoint))
    {
        // Clamp the cursor inside the canvas bounds
        float edgeBuffer = 10f;
        Vector2 clampedPos = new Vector2(
        Mathf.Clamp(localPoint.x, canvasRect.rect.xMin + edgeBuffer, canvasRect.rect.xMax - edgeBuffer),
        Mathf.Clamp(localPoint.y, canvasRect.rect.yMin + edgeBuffer, canvasRect.rect.yMax - edgeBuffer));

        customCursor.anchoredPosition = clampedPos;
    }
}
}
