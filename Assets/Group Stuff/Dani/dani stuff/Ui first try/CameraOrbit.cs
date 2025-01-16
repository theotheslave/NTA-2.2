using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // The object to follow and circle
    public float distance = 10.0f; // Distance from the target
    public float orbitSpeed = 20.0f; // Speed of orbiting
    public Vector3 offset = Vector3.up; // Vertical offset for the camera position

    void Update()
    {
        if (target == null) return; // Exit if no target is set

        // Calculate the rotation around the target
        float orbitAmount = orbitSpeed * Time.deltaTime;
        transform.RotateAround(target.position + offset, Vector3.up, orbitAmount);

        // Look at the target
        transform.LookAt(target.position + offset);

        // Maintain the desired distance
        Vector3 direction = (transform.position - (target.position + offset)).normalized;
        transform.position = (target.position + offset) + direction * distance;
    }
}
