using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource menuSFX;

    [SerializeField] AudioClip menuMusicTrack;
    [SerializeField] public AudioClip menuSFXClick;

    void Start()
    {
        PlayLoopedAudio(menuMusic, menuMusicTrack);
    }

    void PlayLoopedAudio(AudioSource source, AudioClip clip)
    {
        if (source.clip != clip)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        menuSFX.PlayOneShot(clip);
    }
}
