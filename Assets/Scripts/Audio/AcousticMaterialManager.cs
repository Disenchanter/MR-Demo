using UnityEngine;

public class AcousticMaterialManager : MonoBehaviour
{
    [Header("把你做好的库拖进来")]
    public AcousticMaterialLibrary library;

    [Header("HUD 显示")]
    public bool showHUD = true;

    private int index = 0;

    void Start() { ApplyToAll(); }

    void Update()
    {
        if (library == null || library.presets.Count == 0) return;

        // 数字键 1..9 直达
        for (int k = 0; k < 9; k++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + k) && k < library.presets.Count)
            { index = k; ApplyToAll(); }
        }

        // 空格：下一个；Shift+空格：上一个
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
                  $"[全局材质] {library.presets[index].displayName}  （空格下一项，Shift+空格上一项，1..9直达）");
    }
}
