using UnityEngine;
using System.Collections.Generic;

public class NewCentreBall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;         // Speed of movement for attracting balls
    [SerializeField] private float attractionRange = 5f;   // Range within which balls can be attracted
    [SerializeField] private LayerMask ballLayer;          // Layer mask to filter balls
    [SerializeField] private float maxScale = 2f;          // Maximum scale for growth
    [SerializeField] private float growSpeed = 1f;         // Speed of growth
    [SerializeField] private List<GameObject> objectsToGrow; // Specific objects to grow

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
                Vector3 targetScale = originalScale * maxScale;
                obj.transform.localScale = Vector3.MoveTowards(obj.transform.localScale, targetScale, growSpeed * Time.deltaTime);
                Debug.Log($"Growing Object: {obj.name}");
            }
        }

        Debug.Log($"Triggered growth for {objectsToGrow.Count} objects.");
    }
}
