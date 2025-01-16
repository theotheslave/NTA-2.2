using UnityEngine;

public class ToggleEmissionMaterial : MonoBehaviour
{
    [Header("Primary Material Settings")]
    [Tooltip("The primary material whose emission will be toggled.")]
    public Material primaryMaterial;

    [Tooltip("Emission color for the primary material when enabled.")]
    public Color primaryEmissionColor = Color.white;

    [Tooltip("Emission intensity multiplier for the primary material.")]
    public float primaryEmissionIntensity = 1.0f;

    [Header("Secondary Material Settings")]
    [Tooltip("The secondary material whose emission will also be toggled.")]
    public Material secondaryMaterial;

    [Tooltip("Emission color for the secondary material when enabled.")]
    public Color secondaryEmissionColor = Color.white;

    [Tooltip("Emission intensity multiplier for the secondary material.")]
    public float secondaryEmissionIntensity = 0.5f;

    [Header("Toggle Settings")]
    [Tooltip("Toggle this to turn emission on or off for both materials.")]
    public bool enableEmission = false;

    private bool previousState = false;

    void Update()
    {
        if (enableEmission != previousState)
        {
            ToggleEmission(enableEmission);
            previousState = enableEmission;
        }
    }

    void ToggleEmission(bool enable)
    {
        // Handle primary material
        if (primaryMaterial != null && primaryMaterial.HasProperty("_EmissionColor"))
        {
            if (enable)
            {
                primaryMaterial.EnableKeyword("_EMISSION");
                primaryMaterial.SetColor("_EmissionColor", primaryEmissionColor * primaryEmissionIntensity);
            }
            else
            {
                primaryMaterial.DisableKeyword("_EMISSION");
                primaryMaterial.SetColor("_EmissionColor", Color.black);
            }
        }

        // Handle secondary material
        if (secondaryMaterial != null && secondaryMaterial.HasProperty("_EmissionColor"))
        {
            if (enable)
            {
                secondaryMaterial.EnableKeyword("_EMISSION");
                secondaryMaterial.SetColor("_EmissionColor", secondaryEmissionColor * secondaryEmissionIntensity);
            }
            else
            {
                secondaryMaterial.DisableKeyword("_EMISSION");
                secondaryMaterial.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
