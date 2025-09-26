using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class DistanceLowpass : MonoBehaviour
{
    [Header("监听者（一般拖 Main Camera）")]
    public Transform listener;

    [Header("低通截止频率范围")]
    public float minCutoff = 600f;     // 距离很远时的最低截止
    public float maxCutoff = 22000f;   // 很近时的最高截止

    [Header("作用距离（超过此值按最小截止）")]
    public float maxDistance = 25f;

    [Header("距离到截止频率的映射曲线（0=近,1=远）")]
    public AnimationCurve distanceToCutoff = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private AudioLowPassFilter lpf;

    void Awake()
    {
        lpf = GetComponent<AudioLowPassFilter>();
        if (listener == null && Camera.main != null) listener = Camera.main.transform;
    }

    void Update()
    {
        if (!listener) return;
        float d = Vector3.Distance(transform.position, listener.position);
        float t = Mathf.Clamp01(d / maxDistance);      // 0 近 → 1 远
        float k = distanceToCutoff.Evaluate(t);        // 曲线映射
        lpf.cutoffFrequency = Mathf.Lerp(minCutoff, maxCutoff, k);
    }
}
