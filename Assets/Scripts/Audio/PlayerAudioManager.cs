using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] step;
    [SerializeField] private AudioClip[] swooshAttack;
    [SerializeField] private AudioClip swooshThrow;
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip vehicleStart;   

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        source.PlayOneShot(step[Random.Range(0, step.Length)]);
    }

    public void PlaySwooshMelee()
    {
        source.PlayOneShot(swooshAttack[Random.Range(0, swooshAttack.Length)]);
    }

    public void PlaySwooshThrow()
    {
        source.PlayOneShot(swooshThrow);
    }

    public void PlayCoin()
    {
        source.PlayOneShot(coin);
    }
    
    public void PlayHit()
    {
        source.PlayOneShot(hit);
    }

    public void PlayVehicleStart()
    {
        if (vehicleStart != null)
        {
            source.PlayOneShot(vehicleStart);
        }
    }

    public void PlayHeal()
    {
        source.PlayOneShot(heal);
    }
}
