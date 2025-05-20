using UnityEngine;

public class SoundViisualizer : MonoBehaviour
{
    [Header("Frequency Range (0 to 512)")]
    public int minFreqIndex = 0;
    public int maxFreqIndex = 64;

    [Header("Scaling Settings")]
    public float scaleMultiplier = 50f;
    public float smoothSpeed = 10f;
    public Vector3 baseScale = Vector3.one;

    public AudioSource audioSource;
    private float[] spectrumData = new float[512];

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        int start = Mathf.Clamp(minFreqIndex, 0, spectrumData.Length - 1);
        int end = Mathf.Clamp(maxFreqIndex, start + 1, spectrumData.Length);

        float sum = 0f;
        for (int i = start; i < end; i++)
        {
            sum += spectrumData[i];
        }

        float average = sum / (end - start);
        float intensity = average * scaleMultiplier;
        Vector3 targetScale = baseScale + Vector3.one * intensity;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }
}
