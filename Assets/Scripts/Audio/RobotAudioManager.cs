using UnityEngine;

public class RobotAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip shootGun;
    [SerializeField] private AudioClip groundSmash;
    [SerializeField] private AudioClip[] hit;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
 
    public void PlayShootGun()
    {
        source.PlayOneShot(shootGun);
    }

    public void PlayGroundSmash()
    {
        source.PlayOneShot(groundSmash);
    }

    public void PlayHit()
    {
        source.PlayOneShot(hit[Random.Range(0, hit.Length)]);
    }
}