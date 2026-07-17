using UnityEngine;

public class ObjectsAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] explosion;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        source.PlayOneShot(explosion[Random.Range(0, explosion.Length)]);
    }
}