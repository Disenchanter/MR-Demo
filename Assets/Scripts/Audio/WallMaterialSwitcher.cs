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

        // 空格：下一个；Shift+空格：上一个
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
        { index = (index - 1 + library.presets.Count) % library.presets.Count; ApplyCurrent(); }
        else if (Input.GetKeyDown(KeyCode.Space))
        { index = (index + 1) % library.presets.Count; ApplyCurrent(); }

        // 数字键直达 1..9
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

    public void SetIndex(int i) // UI/Inspector 也可调用
    {
        index = i; ApplyCurrent();
    }

    void OnGUI()
    {
        if (!showHUD || mat == null || mat.preset == null) return;
        GUI.Label(new Rect(10, 10 + 18 * (GetInstanceID() % 5), 600, 20),
                  $"[墙 {name}] 材质: {mat.preset.displayName}  （空格下一项，Shift+空格上一项，1..9直达）");
    }
}
