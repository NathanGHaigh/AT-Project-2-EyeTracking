using UnityEngine;

public class scp173Audio : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip audioClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteAudio()
    {
       StopAudio();
        audioClip = null;
    }

    public void PlayAudio()
    {
        if (audioSource.isPlaying)
            return;
        audioSource.PlayOneShot(audioClip);
    }
    public void StopAudio()
    {
        audioSource.Stop();
    }
}
