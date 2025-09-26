using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class OcclusionByMaterial : MonoBehaviour
{
    [Header("监听者（一般拖 Main Camera）")]
    public Transform listener;

    [Header("遮挡用层（只勾选 Wall 层）")]
    public LayerMask occluderMask;

    [Header("平滑过渡系数（越大越顺滑）")]
    public float smooth = 10f;

    [Header("夹取上/下限（防极端值）")]
    public float minCutoffLimit = 500f;     // 最低截止
    public float maxVolumeLimit = 1.0f;     // 最高音量比例

    private AudioSource src;
    private AudioLowPassFilter lpf;
    private float baseVolume;
    private float baseCutoff;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        lpf = GetComponent<AudioLowPassFilter>();
        baseVolume = src.volume;
        baseCutoff = (lpf.cutoffFrequency > 0 ? lpf.cutoffFrequency : 22000f);
        if (listener == null && Camera.main != null) listener = Camera.main.transform;
    }

    void Update()
    {
        if (!listener) return;

        Vector3 dir = listener.position - transform.position;
        float dist  = dir.magnitude;

        // 收集沿线所有命中（多面墙叠加）
        Ray ray = new Ray(transform.position, dir.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, dist, occluderMask);

        float targetCutoff = baseCutoff;
        float targetVolume = baseVolume;

        if (hits != null && hits.Length > 0)
        {
            float accumCutoff = baseCutoff;
            float accumVolume = baseVolume;

            foreach (var h in hits)
            {
                var am = h.collider.GetComponent<AcousticMaterial>();
                if (am != null)
                {
                    accumCutoff = Mathf.Min(accumCutoff, am.cutoffHz);
                    float dbScale = Mathf.Pow(10f, am.extraDb / 20f);
                    accumVolume *= (am.volumeScale * dbScale);
                }
                else
                {
                    // 未挂材质时给个保守默认
                    accumCutoff = Mathf.Min(accumCutoff, 1200f);
                    accumVolume *= 0.7f;
                }
            }

            targetCutoff = Mathf.Max(accumCutoff, minCutoffLimit);
            targetVolume = Mathf.Min(accumVolume, maxVolumeLimit);
        }

        // 平滑过渡
        src.volume = Mathf.Lerp(src.volume, targetVolume, Time.deltaTime * smooth);
        lpf.cutoffFrequency = Mathf.Lerp(lpf.cutoffFrequency, targetCutoff, Time.deltaTime * smooth);
    }
}
