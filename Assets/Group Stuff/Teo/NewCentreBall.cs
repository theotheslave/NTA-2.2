using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class NewCentreBall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;         // Speed of movement for attracting balls
    [SerializeField] private float attractionRange = 5f;   // Range within which balls can be attracted
    [SerializeField] private LayerMask ballLayer;          // Layer mask to filter balls
    [SerializeField] private float maxScale = 2f;          // Maximum scale for growth
    [SerializeField] private float growSpeed = 1f;         // Speed of growth
    [SerializeField] private List<GameObject> objectsToGrow; // Specific objects to grow
    [SerializeField] private List<GameObject> particles;
    private Vector3 originalScale;                         // Original scale of the attractor
    private DesPos desPos;                                 // Reference to DesPos script
    private GameObject currentBall = null;                 // The ball currently being attracted

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale

        // Try to get the DesPos component attached to this GameObject
        desPos = GetComponent<DesPos>();
        if (desPos == null)
        {
            Debug.LogError("DesPos script not found on the attractor. Please attach it.");
        }
    }

    void Update()
    {
        // Check if the attractor is allowed to center objects
        if (desPos == null || !desPos.CanCentre())
        {
            return; // Exit if centering is not allowed or DesPos is missing
        }

        if (currentBall == null)
        {
            // Find a new ball to attract
            Collider[] nearbyBalls = Physics.OverlapSphere(transform.position, attractionRange, ballLayer);

            foreach (Collider col in nearbyBalls)
            {
                if (col.CompareTag("Ball"))
                {
                    currentBall = col.gameObject;
                    break; // Stop after finding the first ball
                }
            }
        }

        // Attract the ball if one is being processed
        if (currentBall != null)
        {
            MoveBallTowardsCenter(currentBall);
        }
    }

    private void MoveBallTowardsCenter(GameObject ball)
    {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();

        if (ballRb != null)
        {
            Vector3 direction = (transform.position - ball.transform.position).normalized;
            ballRb.linearVelocity = direction * moveSpeed;

            Debug.Log($"Moving Ball: {ball.name}, Current Position: {ball.transform.position}, Target Position: {transform.position}");

            // Check if the ball has reached the center
            if (Vector3.Distance(ball.transform.position, transform.position) < 0.1f)
            {
                ballRb.linearVelocity = Vector3.zero; // Stop the ball
                ballRb.isKinematic = true;
                TriggerGrowthOnSpecificObjects(); // Trigger specific objects to grow
                ActivateParticles();
                // Mark the attractor as centered in DesPos
                if (desPos != null)
                {
                    desPos.Centred();
                }

                currentBall = null; // Clear the current ball reference to prevent reprocessing
            }
        }
    }

    private void TriggerGrowthOnSpecificObjects()
    {
        foreach (GameObject obj in objectsToGrow)
        {
            if (obj != null)
            {
                // Ensure the object is ready for scaling
                if (obj.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.isKinematic = true;
                }
                if (obj.TryGetComponent<Animator>(out var animator))
                {
                    animator.enabled = false;
                }

                // Calculate target scale
                Vector3 targetScale = obj.transform.localScale * maxScale;
                Debug.Log($"Starting growth for {obj.name}: Current {obj.transform.localScale}, Target {targetScale}");

                // Scale the object over time
                StartCoroutine(ScaleObjectOverTime(obj, targetScale));
            }
        }
    }

    private void ActivateParticles()
    {

        foreach(GameObject obj in particles)
        {

            obj.SetActive(true);
        }

    }

    private IEnumerator ScaleObjectOverTime(GameObject obj, Vector3 targetScale)
    {
        while (Vector3.Distance(obj.transform.localScale, targetScale) > 0.01f)
        {
            float step = growSpeed * Time.deltaTime;
            obj.transform.localScale = Vector3.MoveTowards(obj.transform.localScale, targetScale, step);
            Debug.Log($"[Scaling] {obj.name}: Current {obj.transform.localScale}, Target {targetScale}");
            yield return null; // Wait for the next frame
        }

        obj.transform.localScale = targetScale; // Snap to target
        Debug.Log($"Scaling complete for {obj.name} to {targetScale}");
    }
}
