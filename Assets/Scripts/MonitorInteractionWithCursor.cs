using UnityEngine;
using UnityEngine.UI;

public class MonitorInteractionWithCursor : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject monitor;
    public Canvas monitorCanvas;
    public RectTransform customCursor;
    public float interactionDistance = 3f;

    private bool isCursorActive = false;

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
        }

        if (isCursorActive)
        {
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
