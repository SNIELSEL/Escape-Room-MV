using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class StairsTeleport : MonoBehaviour
{
    public Image fadeInImage;
    public Volume postProccesing;
    private Vignette vignette;

    bool teleporting;
    bool ready;
    Transform loc;
    public void Teleport(Transform location)
    {
        loc = location;
        teleporting = true;
    }

    public void Update()
    {
        if(teleporting)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (postProccesing.profile.TryGet(out vignette) && !ready)
            {
                vignette.intensity.value += Time.deltaTime * 0.8f;
                vignette.intensity.value = Mathf.Clamp01(vignette.intensity.value);
                fadeInImage.color += new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, Time.deltaTime * 0.8f);
            }

            if (fadeInImage.color.a >= 1)
            {
                ready = true;

                player.transform.position = new Vector3(player.transform.position.x, loc.position.y, player.transform.position.z);

                if (postProccesing.profile.TryGet(out vignette))
                {
                    fadeInImage.color -= new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, Time.deltaTime * 0.8f);
                    vignette.intensity.value -= Time.deltaTime * 0.8f;
                    vignette.intensity.value = Mathf.Clamp01(vignette.intensity.value);
                }
            }

            if (fadeInImage.color.a <= 0 && ready)
            {
                teleporting = false;
                ready = false;
            }
            else if(fadeInImage.color.a >= 0 && ready)
            {
                if (postProccesing.profile.TryGet(out vignette))
                {
                    fadeInImage.color -= new Color(fadeInImage.color.r, fadeInImage.color.g, fadeInImage.color.b, Time.deltaTime * 0.8f);
                    vignette.intensity.value -= Time.deltaTime * 0.8f;
                    vignette.intensity.value = Mathf.Clamp01(vignette.intensity.value);
                }
            }
        }
    }
}
