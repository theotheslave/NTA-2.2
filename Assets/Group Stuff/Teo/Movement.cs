using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; 
    [SerializeField] private float moveSpeed = 2f;  
    

    private int currentWaypointIndex = 0; 
    private bool hasReachedEnd = false;
    void Update()
    {
        if (hasReachedEnd || waypoints.Length == 0) return;

        
        Transform targetWaypoint = waypoints[currentWaypointIndex];

       
        MoveTowards(targetWaypoint);

        
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
               
                currentWaypointIndex++;
            }
            else
            {
               
                hasReachedEnd = true;
                Debug.Log("Reached the end of the path.");
            }
        }
    }

    private void MoveTowards(Transform targetWaypoint)
    {
        
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        
        
    }
}