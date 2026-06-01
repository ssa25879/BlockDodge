using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioClip gameBGM;
    public AudioClip deadBGM;

    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayGameBGM();
    }

    public void PlayGameBGM()
    {
        audioSource.clip = gameBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDeadBGM()
    {
        audioSource.clip = deadBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}
