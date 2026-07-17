using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip click;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic()
    {

    }

    public void PlayClick()
    {
        sfxSource.PlayOneShot(click);
    }
}