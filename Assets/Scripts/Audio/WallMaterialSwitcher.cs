using UnityEngine;

[RequireComponent(typeof(AcousticMaterial))]
public class WallMaterialSwitcher : MonoBehaviour
{
    public AcousticMaterialLibrary library;
    public int index = 0;
    public bool showHUD = true;

    private AcousticMaterial mat;

    void Awake()
    {
        mat = GetComponent<AcousticMaterial>();
        ApplyCurrent();
    }

    void Update()
    {
        if (library == null || library.presets.Count == 0 || mat == null) return;

    // Space: next; Shift+Space: previous
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
        { index = (index - 1 + library.presets.Count) % library.presets.Count; ApplyCurrent(); }
        else if (Input.GetKeyDown(KeyCode.Space))
        { index = (index + 1) % library.presets.Count; ApplyCurrent(); }

    // Number keys 1..9 for direct jump
        for (int k = 0; k < 9; k++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + k) && k < library.presets.Count)
            { index = k; ApplyCurrent(); }
        }
    }

    void ApplyCurrent()
    {
        if (library != null && library.presets.Count > 0)
        {
            int i = Mathf.Clamp(index, 0, library.presets.Count - 1);
            var p = library.presets[i];
            mat.ApplyPreset(p);
            Debug.Log($"[Wall:{name}] Switch to: {p.displayName}");
        }
    }

    public void SetIndex(int i) // Callable from UI/Inspector
    {
        index = i; ApplyCurrent();
    }

    void OnGUI()
    {
        if (!showHUD || mat == null || mat.preset == null) return;
    GUI.Label(new Rect(10, 10 + 18 * (GetInstanceID() % 5), 600, 20),
          $"[Wall {name}] Material: {mat.preset.displayName}  (Space for next, Shift+Space for previous, 1..9 to jump)");
    }
}
