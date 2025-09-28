using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class DistanceLowpass : MonoBehaviour
{
    [Header("Listener (usually drag Main Camera)")]
    public Transform listener;

    [Header("Low-pass cutoff range")]
    public float minCutoff = 600f;     // Lowest cutoff at far distance
    public float maxCutoff = 22000f;   // Highest cutoff when very close

    [Header("Effective distance (beyond this uses min cutoff)")]
    public float maxDistance = 25f;

    [Header("Distance-to-cutoff curve (0=near,1=far)")]
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
    float t = Mathf.Clamp01(d / maxDistance);      // 0 near -> 1 far
    float k = distanceToCutoff.Evaluate(t);        // Curve mapping
        lpf.cutoffFrequency = Mathf.Lerp(minCutoff, maxCutoff, k);
    }
}
