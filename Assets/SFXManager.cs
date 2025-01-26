using UnityEngine;

public class SFXManager : MonoBehaviour
{
public static SFXManager Instance { get; private set; }

    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayOneShot(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
