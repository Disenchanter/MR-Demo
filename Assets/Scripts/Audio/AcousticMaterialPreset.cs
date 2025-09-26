using UnityEngine;

[CreateAssetMenu(fileName = "AcousticMatPreset", menuName = "Audio/Acoustic Material Preset")]
public class AcousticMaterialPreset : ScriptableObject
{
    [Header("显示名（仅用于UI提示）")]
    public string displayName = "Concrete";

    [Header("可视化颜色")]
    public Color color = Color.gray;

    [Header("遮挡时的参数")]
    [Range(0f, 1f)] public float volumeScale = 0.7f; // 线性比例
    public float cutoffHz = 1200f;                   // 低通截止
    [Range(-24f, 0f)] public float extraDb = 0f;     // 附加dB衰减(可选)
}
