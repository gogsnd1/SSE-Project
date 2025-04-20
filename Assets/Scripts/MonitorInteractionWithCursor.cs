using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class MonitorInteractionWithCursor : MonoBehaviour
{
   public Camera playerCamera;
   public GameObject monitor;
   public Canvas monitorCanvas;
   public RectTransform customCursor;
   public LookScript lookScript;
   public float interactionDistance = 3f;
   private bool isCursorActive = false;
   private Vector2 cursorPosition;
   private float cursorActivationTime = -1f;
   [SerializeField][Range(0f, 1f)] float inputCooldown = 0.1f; // ignore input for s seconds after activating

   [Header("Cursor Settings")]
   [SerializeField][Range(0f, 1f)] float cursorSensitivity = 0.5f; // sensitivity for cursor movement
    [SerializeField][Range(0f, 10f)] float edgeBuffer = 5f; // buffer to prevent cursor from going off-screen


   [Header("Click Sound Settings")]
   public AudioMixer audioMixer; // Assign AudioMixer in Inspector
   public AudioSource audioSource;
   [SerializeField] AudioClip clickSound;         // Assign sound in Inspector

    [Header("Camera lock Settings")]
    public float maxYaw = 15f;   // Left-right limit (Y-axis)
    public float maxPitch = 10f; // Up-down limit (X-axis)
    private Vector3 initialCameraRotation;
    // Vertical limit (X axis)


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
        cursorActivationTime = Time.time; // record when cursor was toggled
        return;
    }

    if (isCursorActive)
    {
        lookScript.playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        lookScript.enabled = false;

        // Check if enough time has passed since cursor activation
        if (Time.time - cursorActivationTime > inputCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {
               audioSource.PlayOneShot(clickSound); // Let the AudioMixer control volume
            }
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

       if (isCursorActive)
    {
    initialCameraRotation = playerCamera.transform.localEulerAngles;
    // Center the cursor when monitor opens
    cursorPosition = Vector2.zero;
    customCursor.anchoredPosition = Vector2.zero;
    //lock the camera rotation
    //lookScript.rotationX += (invert ? 1 : -1) * mouseY;
    //lookScript.rotationX = Mathf.Clamp(rotationX, -90, 90);
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