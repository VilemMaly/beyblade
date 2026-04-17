using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class kamera : MonoBehaviour
{
    public float sensitivity = 0.1f;
    public Transform playerBody;

    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;

    public float minHorizontalAngle = -90f;
    public float maxHorizontalAngle = 90f;

    [Header("Smoothing")]
    public float smoothTime = 0.05f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private Vector2 lookInput;
    private Vector2 currentLook;
    private Vector2 currentLookVelocity;

    public InputActionReference lookAction;

    // --- ZOOM ---
    private Camera cam;
    private float defaultFOV;
    private Coroutine zoomCoroutine;

    void OnEnable()
    {
        lookAction.action.Enable();
    }

    void OnDisable()
    {
        lookAction.action.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yRotation = playerBody.eulerAngles.y;

        cam = GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {
        lookInput = lookAction.action.ReadValue<Vector2>();

        Vector2 targetLook = lookInput * sensitivity;

        currentLook = Vector2.SmoothDamp(
            currentLook,
            targetLook,
            ref currentLookVelocity,
            smoothTime
        );

        float mouseX = currentLook.x;
        float mouseY = currentLook.y;

        // vertikální
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // horizontální
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, minHorizontalAngle, maxHorizontalAngle);
        playerBody.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    // =========================
    // ZOOM FUNKCE
    // =========================

    public void ZoomTo(float targetFOV, float duration)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomCoroutine(targetFOV, duration));
    }

    public void ResetZoom(float duration)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomCoroutine(defaultFOV, duration));
    }

    private float fovVelocity; // musí být mimo coroutine

    private IEnumerator ZoomCoroutine(float targetFOV, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            cam.fieldOfView = Mathf.SmoothDamp(
                cam.fieldOfView,
                targetFOV,
                ref fovVelocity,
                duration
            );

            yield return null;
        }

        cam.fieldOfView = targetFOV;
        fovVelocity = 0f;
    }
}