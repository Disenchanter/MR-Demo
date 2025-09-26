using UnityEngine;

[DisallowMultipleComponent]
public class AcousticMaterial : MonoBehaviour
{
    [Header("当前使用的预设")]
    public AcousticMaterialPreset preset;

    // 提供给遮挡脚本读取的“只读快照”（运行时会同步更新）
    [HideInInspector] public float volumeScale = 0.7f;
    [HideInInspector] public float cutoffHz   = 1200f;
    [HideInInspector] public float extraDb    = 0f;
    [HideInInspector] public string displayName = "Unset";

    MeshRenderer mr;
    MaterialPropertyBlock mpb;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        if (mpb == null) mpb = new MaterialPropertyBlock();
        ApplyPreset(preset);
    }

    void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        if (mr == null) mr = GetComponent<MeshRenderer>();
        if (mpb == null) mpb = new MaterialPropertyBlock();
        ApplyPreset(preset);
    }

    public void ApplyPreset(AcousticMaterialPreset p)
    {
        if (p == null || mr == null) return;
        preset = p;

        // 同步参数快照（遮挡脚本直接读快照，避免频繁GetComponent）
        volumeScale = p.volumeScale;
        cutoffHz    = p.cutoffHz;
        extraDb     = p.extraDb;
        displayName = string.IsNullOrWhiteSpace(p.displayName) ? name : p.displayName;

        // 仅通过 PropertyBlock 改颜色，不创建/替换材质资源（避免编辑器断言/材质实例泛滥）
        mr.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", p.color); // URP/Lit
        mpb.SetColor("_Color",     p.color); // 内置Standard
        mr.SetPropertyBlock(mpb);
    }
}
