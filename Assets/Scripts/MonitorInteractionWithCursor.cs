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
   [SerializeField][Range(0f, 1f)] float cursorSensitivity = 0.5f; // sensitivity for cursor movement
    [SerializeField][Range(0f, 10f)] float edgeBuffer = 5f; // buffer to prevent cursor from going off-screen


   [Header("Click Sound Settings")]
   public AudioMixer audioMixer; // Assign AudioMixer in Inspector
   public AudioSource audioSource;
   [SerializeField] AudioClip clickSound;         // Assign sound in Inspector


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
        playerCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, -90f, 0);
        lookScript.enabled = false;
        {
            if (Input.GetMouseButtonDown(0))
            {
               audioSource.PlayOneShot(clickSound); // Let the AudioMixer control volume
            }
        UpdateCursorPosition();
    }
    }
}

   void ToggleCustomCursor()
   {
       isCursorActive = !isCursorActive;
      
       Cursor.visible = false;
       Cursor.lockState = (isCursorActive ? CursorLockMode.None : CursorLockMode.Locked);
       customCursor.gameObject.SetActive(isCursorActive);

       if (isCursorActive)
    {
    // Center the cursor when monitor opens
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
       // Clamp the cursor inside the canvas bounds
       Vector2 clampedPos = new Vector2(
       Mathf.Clamp(localPoint.x * cursorSensitivity, canvasRect.rect.xMin + edgeBuffer, canvasRect.rect.xMax - edgeBuffer),
       Mathf.Clamp(localPoint.y * cursorSensitivity, canvasRect.rect.yMin + edgeBuffer, canvasRect.rect.yMax - edgeBuffer));


       customCursor.anchoredPosition = clampedPos;
   }
}
}