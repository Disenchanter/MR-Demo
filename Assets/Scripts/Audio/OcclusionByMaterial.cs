using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class OcclusionByMaterial : MonoBehaviour
{
    [Header("Listener (usually drag Main Camera)")]
    public Transform listener;

    [Header("Occluder layers (check Wall layer only)")]
    public LayerMask occluderMask;

    [Header("Smoothing factor (larger is smoother)")]
    public float smooth = 10f;

    [Header("Clamp limits (prevent extremes)")]
    public float minCutoffLimit = 500f;     // Minimum cutoff
    public float maxVolumeLimit = 1.0f;     // Maximum volume ratio

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

    // Collect all hits along the ray (multiple walls stack)
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
                    // Use conservative defaults when no material is attached
                    accumCutoff = Mathf.Min(accumCutoff, 1200f);
                    accumVolume *= 0.7f;
                }
            }

            targetCutoff = Mathf.Max(accumCutoff, minCutoffLimit);
            targetVolume = Mathf.Min(accumVolume, maxVolumeLimit);
        }

    // Smooth interpolation
        src.volume = Mathf.Lerp(src.volume, targetVolume, Time.deltaTime * smooth);
        lpf.cutoffFrequency = Mathf.Lerp(lpf.cutoffFrequency, targetCutoff, Time.deltaTime * smooth);
    }
}
