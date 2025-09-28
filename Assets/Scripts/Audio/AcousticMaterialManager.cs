using UnityEngine;

public class AcousticMaterialManager : MonoBehaviour
{
    [Header("Drag your prepared library here")]
    public AcousticMaterialLibrary library;

    [Header("HUD Display")]
    public bool showHUD = true;

    private int index = 0;

    void Start() { ApplyToAll(); }

    void Update()
    {
        if (library == null || library.presets.Count == 0) return;

    // Number keys 1..9 jump directly
        for (int k = 0; k < 9; k++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + k) && k < library.presets.Count)
            { index = k; ApplyToAll(); }
        }

    // Space: next; Shift+Space: previous
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
        { index = (index - 1 + library.presets.Count) % library.presets.Count; ApplyToAll(); }
        else if (Input.GetKeyDown(KeyCode.Space))
        { index = (index + 1) % library.presets.Count; ApplyToAll(); }
    }

    void ApplyToAll()
    {
        var p = library.presets[Mathf.Clamp(index, 0, library.presets.Count - 1)];
        var all = FindObjectsOfType<AcousticMaterial>();
        foreach (var m in all) m.ApplyPreset(p);
        Debug.Log($"[Global] Switch all walls to: {p.displayName}");
    }

    public void SetIndex(int i) { index = Mathf.Clamp(i, 0, library.presets.Count - 1); ApplyToAll(); }

    void OnGUI()
    {
        if (!showHUD || library == null || library.presets.Count == 0) return;
    GUI.Label(new Rect(10, 10, 560, 20),
          $"[Global Material] {library.presets[index].displayName}  (Space for next, Shift+Space for previous, 1..6 to jump)");
    }
}
