using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip swooshThrow;
    [SerializeField] private AudioClip gunFire;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySwooshThrow()
    {
        source.PlayOneShot(swooshThrow);
    }

    public void PlayGunFire()
    {
        source.PlayOneShot(gunFire);
    }
}