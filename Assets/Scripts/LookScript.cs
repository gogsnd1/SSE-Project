using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookScript : MonoBehaviour
{
    [SerializeField] bool invert = false;
    [SerializeField] float sensitivity = 10.0f;

    [SerializeField] Camera playerCamera;
    [SerializeField] Transform playerBody;

    [Header("Click Sound Settings")]
    [SerializeField] AudioClip clickSound;         // Assign sound in Inspector
    [SerializeField][Range(0f, 1f)] float clickVolume = 0.5f;  // Adjustable volume (0 to 1)

    private AudioSource audioSource;

    private float rotationX = 0;
    private float rotationY = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Setup AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationY += mouseX;
        rotationX += (invert ? 1 : -1) * mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        // Play click sound with volume
        if (Input.GetMouseButtonDown(0) && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, clickVolume);
        }
    }
}
