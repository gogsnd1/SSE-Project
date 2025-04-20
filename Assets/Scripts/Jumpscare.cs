using System.Collections;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [Header("Jumpscare Setup")]
    public GameObject scareObject;
    public AudioSource scareAudio;

    [Header("Timing")]
    public float scareObjectDuration = 1.5f;

    [Header("Camera Shake")]
    public Camera targetCamera;
    public float shakeIntensity = 0.3f;
    public float shakeSpeed = 20f;

    private Vector3 originalCamPos;
    private bool isScaring = false;
    private bool isShaking = false;

    void Start()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("No camera assigned. Using Main Camera.");
            targetCamera = Camera.main;
        }

        if (targetCamera != null)
            originalCamPos = targetCamera.transform.localPosition;

        TriggerJumpscare();
    }

    public void TriggerJumpscare()
    {
        if (isScaring) return;
        isScaring = true;

        if (scareObject != null)
        {
            scareObject.SetActive(true);
            StartCoroutine(HideScareObjectAfterTime(scareObjectDuration));
        }

        if (scareAudio != null && scareAudio.clip != null)
        {
            scareAudio.Play();
        }

        if (targetCamera != null)
        {
            isShaking = true;
            StartCoroutine(CameraShake());
        }
    }

    IEnumerator HideScareObjectAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (scareObject != null)
            scareObject.SetActive(false);

        // Stop the shake
        isShaking = false;
        ResetCameraPosition();

        isScaring = false;
    }

    IEnumerator CameraShake()
    {
        while (isShaking)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            if (targetCamera != null)
                targetCamera.transform.localPosition = originalCamPos + new Vector3(x, y, 0f);

            yield return new WaitForSeconds(1f / shakeSpeed);
        }

        ResetCameraPosition();
    }

    void ResetCameraPosition()
    {
        if (targetCamera != null)
            targetCamera.transform.localPosition = originalCamPos;
    }
}
