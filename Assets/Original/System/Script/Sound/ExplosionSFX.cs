using UnityEngine;

public class ExplosionSFX : MonoBehaviour
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip[] clips;

    void OnEnable()
    {
        var clip = clips[Random.Range(0, clips.Length)];
        sfx.pitch = Random.Range(0.95f, 1.05f);
        sfx.spatialBlend = 1f;
        sfx.minDistance = 10f;
        sfx.maxDistance = 500f;

        sfx.PlayOneShot(clip);
    }
}