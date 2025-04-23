using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class MonitorInteractionWithCursor : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject player;
    public GameObject monitor;
    public Canvas monitorCanvas;
    public RectTransform customCursor;
    public LookScript lookScript;
    public float interactionDistance = 3f;
    private bool isCursorActive = false;
    private Vector2 cursorPosition;

    [Header("Cursor Settings")]
    [SerializeField][Range(0.1f, 1f)] float cursorSize = 1f;

    [SerializeField][Range(0f, 1f)] float cursorSensitivity = 0.5f;
    [SerializeField][Range(0f, 10f)] float edgeBuffer = 5f;

    [Header("Click Sound Settings")]
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    [SerializeField] AudioClip clickSound;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        customCursor.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!PauseMenu.GameIsPaused && !isCursorActive)
        {
            lookScript.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleCustomCursor();
            return;
        }

        if (isCursorActive)
        {
            // Set player rotation directly
            player.transform.rotation = Quaternion.Euler(0, -90f, 0);

            // Reset camera exactly
            playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);

            // Sync with LookScript internal values
            lookScript.ResetLookRotation(player.transform.eulerAngles, Vector3.zero);

            lookScript.enabled = false;

            if (Input.GetMouseButtonDown(0))
            {
                audioSource.PlayOneShot(clickSound);
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
        customCursor.localScale = Vector3.one * cursorSize;


        if (isCursorActive)
        {
            cursorPosition = Vector2.zero;
            customCursor.anchoredPosition = Vector2.zero;
            lookScript.enabled = false;
        }
    }

    void UpdateCursorPosition()
    {
        Vector2 localPoint;
        RectTransform canvasRect = monitorCanvas.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, Input.mousePosition, playerCamera, out localPoint))
        {
            Vector2 clampedPos = new Vector2(
                Mathf.Clamp(localPoint.x * cursorSensitivity, canvasRect.rect.xMin + edgeBuffer, canvasRect.rect.xMax - edgeBuffer),
                Mathf.Clamp(localPoint.y * cursorSensitivity, canvasRect.rect.yMin + edgeBuffer, canvasRect.rect.yMax - edgeBuffer));

            customCursor.anchoredPosition = clampedPos;
        }
    }
}