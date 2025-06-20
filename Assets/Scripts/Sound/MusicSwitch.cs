using UnityEngine;
using System.Collections;

public class MusicSwitch : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource sourceAudio;
    [SerializeField] AudioClip[] sourceClip;

    [Header("Settings")]
    [SerializeField] float fadeDuration = 1f;

    bool isSwitching = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isSwitching) return;

        if (other.CompareTag("Lab"))
        {
            StartCoroutine(SwitchMusic(sourceClip[0]));
        }

        else if (other.CompareTag("Hallway"))
        {
            StartCoroutine(SwitchMusic(sourceClip[1]));
        }

        else if (other.CompareTag("ClassRoom"))
        {
            StartCoroutine(SwitchMusic(sourceClip[2]));
        }
    }

    IEnumerator SwitchMusic(AudioClip newClip)
    {
        isSwitching = true;

        float startVolume = sourceAudio.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            sourceAudio.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        sourceAudio.volume = 0;
        sourceAudio.clip = newClip;
        sourceAudio.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            sourceAudio.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        sourceAudio.volume = startVolume;
        isSwitching = false;
    }
}
