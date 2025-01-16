using UnityEngine;

public class CentreTutorial : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;        
    [SerializeField] private float attractionRange = 5f;  
    [SerializeField] private LayerMask ballLayer;         
    [SerializeField] private GameObject movementObject;   

    private GameObject currentBall = null;               
    private DesPos desPos;                                

    void Start()
    {
       
        desPos = GetComponent<DesPos>();
        if (desPos == null)
        {
            Debug.LogError("DesPos script not found on the attractor. Please attach it.");
        }
    }

    void Update()
    {
       
        if (desPos != null && !desPos.CanCentre())
        {
            return; 
        }

        
        if (currentBall == null)
        {
            Collider[] nearbyBalls = Physics.OverlapSphere(transform.position, attractionRange, ballLayer);
            foreach (Collider col in nearbyBalls)
            {
                if (col.CompareTag("Ball"))
                {
                    currentBall = col.gameObject;
                    break; 
                }
            }
        }

        
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

            
            if (Vector3.Distance(ball.transform.position, transform.position) < 0.1f)
            {
                ballRb.linearVelocity = Vector3.zero; 
                ballRb.isKinematic = true;

                Debug.Log($"{ball.name} reached the center.");

               
                if (desPos != null)
                {
                    desPos.Centred();
                }

               
                StartMovement();

                currentBall = null; 
            }
        }
    }

    private void StartMovement()
    {
        if (movementObject == null)
        {
            Debug.LogWarning("Movement object is not assigned!");
            return;
        }

        Movement movementScript = movementObject.GetComponent<Movement>();
        if (movementScript != null)
        {
            Debug.Log("Starting Movement script on the third object.");
            movementScript.enabled = true; 
        }
        else
        {
            Debug.LogWarning("No Movement script found on the movement object.");
        }
    }
}