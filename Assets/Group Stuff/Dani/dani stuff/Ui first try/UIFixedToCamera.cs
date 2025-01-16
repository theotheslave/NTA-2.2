using UnityEngine;

public class UIFixedToCamera : MonoBehaviour
{
    [Tooltip("The camera to which the UI will be fixed.")]
    public Camera targetCamera;

    [Tooltip("The position on the screen for the UI element (0 to 1 in Viewport coordinates).")]
    public Vector2 screenPosition = new Vector2(0.5f, 0.5f);

    [Tooltip("Distance from the camera where the UI will appear.")]
    public float distanceFromCamera = 5.0f;

    public GameObject outerCircle;
    public GameObject innerDot;

    void Start()
    {
        if (targetCamera == null)
        {
            // Default to the main camera if not assigned
            targetCamera = Camera.main;
        }

        if (outerCircle == null || innerDot == null)
        {
            Debug.LogError("OuterCircle and InnerDot must be assigned!");
        }
    }

    void LateUpdate()
    {
        // Calculate the world position based on the camera's viewport
        Vector3 worldPosition = targetCamera.ViewportToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, distanceFromCamera)
        );

        // Position the elements
        if (outerCircle != null) outerCircle.transform.position = worldPosition;
        if (innerDot != null) innerDot.transform.position = worldPosition;

        // Lock rotation to always face the camera
        if (outerCircle != null) outerCircle.transform.rotation = targetCamera.transform.rotation;
        if (innerDot != null) innerDot.transform.rotation = targetCamera.transform.rotation;
    }
}
