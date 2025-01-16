using UnityEngine;

public class UIExpansionController3D : MonoBehaviour
{
    public GameObject outerCircle;
    public GameObject innerDot;

    [Tooltip("Speed of the inner dot's expansion and collapse.")]
    public float speed = 0.5f;

    [Tooltip("Max size multiplier when the inner dot reaches the outer circle.")]
    public float maxSizeMultiplier = 1.0f;

    private Vector3 originalScale;
    private float targetScale;
    private bool isExpanding = true;

    void Start()
    {
        if (outerCircle == null || innerDot == null)
        {
            Debug.LogError("Outer Circle or Inner Dot is not assigned!");
            return;
        }

        // Save the initial scale of the inner dot
        originalScale = innerDot.transform.localScale;

        // Set the target scale to match the outer circle size
        targetScale = outerCircle.transform.localScale.x * maxSizeMultiplier;
    }

    void Update()
    {
        if (isExpanding)
        {
            // Gradually increase the size
            innerDot.transform.localScale = Vector3.MoveTowards(
                innerDot.transform.localScale,
                new Vector3(targetScale, targetScale, originalScale.z),
                speed * Time.deltaTime
            );

            // Switch to collapsing when fully expanded
            if (innerDot.transform.localScale.x >= targetScale)
            {
                isExpanding = false;
            }
        }
        else
        {
            // Gradually decrease the size
            innerDot.transform.localScale = Vector3.MoveTowards(
                innerDot.transform.localScale,
                originalScale,
                speed * Time.deltaTime
            );

            // Switch to expanding when fully collapsed
            if (innerDot.transform.localScale.x <= originalScale.x)
            {
                isExpanding = true;
            }
        }
    }

    // Method to dynamically change the outer and inner circle materials
    public void SetCustomMaterials(Material outerMaterial, Material innerMaterial)
    {
        if (outerMaterial != null) outerCircle.GetComponent<Renderer>().material = outerMaterial;
        if (innerMaterial != null) innerDot.GetComponent<Renderer>().material = innerMaterial;
    }
}