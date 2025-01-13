using UnityEngine;

public class UICamera : MonoBehaviour
{
    public Transform playerCamera;

    void Update()
    {
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera);
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up); // Keep upright
        }
    }
}
