using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AcousticMatLibrary", menuName = "Audio/Acoustic Material Library")]
public class AcousticMaterialLibrary : ScriptableObject
{
    public List<AcousticMaterialPreset> presets = new List<AcousticMaterialPreset>();
}
