using UnityEngine;

[DisallowMultipleComponent]
public class AcousticMaterial : MonoBehaviour
{
    [Header("Current Preset")]
    public AcousticMaterialPreset preset;

    // Read-only snapshot for occlusion scripts (kept in sync at runtime)
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

    // Sync parameter snapshot so occlusion scripts can read it without frequent GetComponent calls
        volumeScale = p.volumeScale;
        cutoffHz    = p.cutoffHz;
        extraDb     = p.extraDb;
        displayName = string.IsNullOrWhiteSpace(p.displayName) ? name : p.displayName;

    // Change color via PropertyBlock only to avoid creating/replacing material assets (prevents editor assertions/material proliferation)
        mr.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", p.color); // URP/Lit
    mpb.SetColor("_Color",     p.color); // Built-in Standard
        mr.SetPropertyBlock(mpb);
    }
}
