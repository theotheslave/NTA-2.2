using UnityEngine;

public class UIjustexpanidng : MonoBehaviour
{
    public GameObject outerCircle;
    public GameObject innerDot;

    [Tooltip("Speed of the inner dot's expansion.")]
    public float expansionSpeed = 0.5f;

    [Tooltip("Max size when the inner dot reaches the outer circle.")]
    public float maxSize = 2.0f;

    private Vector3 initialScale;
    private float currentScale = 0.1f;

    void Start()
    {
        if (outerCircle == null || innerDot == null)
        {
            Debug.LogError("Outer Circle or Inner Dot is not assigned!");
            return;
        }

        // Get the initial scale of the Inner Dot
        initialScale = innerDot.transform.localScale;
        currentScale = initialScale.x;
    }

    void Update()
    {
        if (currentScale < maxSize)
        {
            currentScale += expansionSpeed * Time.deltaTime;
            innerDot.transform.localScale = new Vector3(currentScale, currentScale, 1f);
        }
    }

    // Method to dynamically change the outer and inner circle materials
    public void SetCustomMaterials(Material outerMaterial, Material innerMaterial)
    {
        if (outerMaterial != null) outerCircle.GetComponent<Renderer>().material = outerMaterial;
        if (innerMaterial != null) innerDot.GetComponent<Renderer>().material = innerMaterial;
    }
}