using UnityEngine;

[CreateAssetMenu(fileName = "AcousticMatPreset", menuName = "Audio/Acoustic Material Preset")]
public class AcousticMaterialPreset : ScriptableObject
{
    [Header("Display Name (UI hint only)")]
    public string displayName = "Concrete";

    [Header("Visualization Color")]
    public Color color = Color.gray;

    [Header("Occlusion Parameters")]
    [Range(0f, 1f)] public float volumeScale = 0.7f; // Linear ratio
    public float cutoffHz = 1200f;                   // Low-pass cutoff
    [Range(-24f, 0f)] public float extraDb = 0f;     // Additional dB attenuation (optional)
}
