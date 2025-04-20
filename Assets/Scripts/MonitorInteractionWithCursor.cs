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
   private Vector2 cursorPosition;

   [Header("Cursor Settings")]
   [SerializeField][Range(0f, 1f)] float cursorSensitivity = 0.5f;
    [SerializeField] float snapDistance = 50f; // Distance to snap cursor to button


   [Header("Click Sound Settings")]
   [SerializeField] AudioClip clickSound;         // Assign sound in Inspector
   [SerializeField][Range(0f, 1f)] float clickVolume = 0.5f;

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


       if ( Input.GetKeyDown(KeyCode.Space))
       {
           ToggleCustomCursor();
           //TODO: Stop sound from playing when Spacebar is pressed

       }


       if (isCursorActive)
       {
        Vector3 currentEuler = playerCamera.transform.localEulerAngles;

        // Normalize angles to -180 to 180
        float yaw = NormalizeAngle(currentEuler.y);
        float pitch = NormalizeAngle(currentEuler.x);
        float initialYaw = NormalizeAngle(initialCameraRotation.y);
        float initialPitch = NormalizeAngle(initialCameraRotation.x);

        yaw = Mathf.Clamp(yaw, initialYaw - maxYaw, initialYaw + maxYaw);
        pitch = Mathf.Clamp(pitch, initialPitch - maxPitch, initialPitch + maxPitch);

        playerCamera.transform.localEulerAngles = new Vector3(pitch, yaw, 0);
           
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

       if (isCursorActive)
    {
    initialCameraRotation = playerCamera.transform.localEulerAngles;
    // Center the cursor when monitor opens
    cursorPosition = Vector2.zero;
    customCursor.anchoredPosition = Vector2.zero;
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
       float edgeBuffer = 10f;
       Vector2 clampedPos = new Vector2(
       Mathf.Clamp(localPoint.x, canvasRect.rect.xMin + edgeBuffer, canvasRect.rect.xMax - edgeBuffer),
       Mathf.Clamp(localPoint.y, canvasRect.rect.yMin + edgeBuffer, canvasRect.rect.yMax - edgeBuffer));


       customCursor.anchoredPosition = clampedPos;
   }
}

float NormalizeAngle(float angle)
{
    if (angle > 180f) angle -= 360f;
    return angle;
}


Vector2 GetSnappedCursorPosition(Vector2 currentPos)
{
    Button[] buttons = monitorCanvas.GetComponentsInChildren<Button>();
    RectTransform closestButton = null;
    float closestDistance = float.MaxValue;

    foreach (Button btn in buttons)
    {
        RectTransform btnRect = btn.GetComponent<RectTransform>();
        Vector2 btnPos = btnRect.anchoredPosition;
        float dist = Vector2.Distance(currentPos, btnPos);

        if (dist < closestDistance && dist <= snapDistance)
        {
            closestDistance = dist;
            closestButton = btnRect;
        }
    }

    // If a button is close enough, snap to it
    if (closestButton != null)
    {
        return closestButton.anchoredPosition;
    }

    // Otherwise return the current position
    return currentPos;
}
}