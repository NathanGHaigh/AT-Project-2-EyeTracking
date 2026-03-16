using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource uiSource;

    public AudioSource altSource;

    public AudioClip slotHover;

    public AudioClip selectedItem;

    public AudioClip pickUpAudio;

    public AudioClip ambientTrack;

    public AudioClip chase096;

    public void HoveringItem()
    {
        uiSource.volume = 0.5f;
        uiSource.PlayOneShot(slotHover);
    }
    public void SelectedItem()
    {
        uiSource.clip = selectedItem;
        uiSource.Play();
    }
    public void PlayAmbientTrack()
    {
        PlayLoopedAudio(ambientTrack, 0.7f);
    }
    public void Play096Chase()
    {
        PlayLoopedAudio(chase096, 0.6f);
    }

    public void PlayPickUpAudio()
    {
        uiSource.PlayOneShot(pickUpAudio);
    }

    public void PlayLoopedAudio(AudioClip clip, float volume = 1f)
    {
        altSource.clip = clip;
        altSource.volume = volume;
        altSource.loop = true;
        altSource.Play();
    }

    public void StopLoopedAudio()
    {
        altSource.Stop();
        altSource.loop = false;
        altSource.clip = null;
    }

    private IEnumerator FadeInCoroutine(AudioClip clip, float targetVol, float duration)
    {
        altSource.clip = clip;
        altSource.volume = 0f;
        altSource.loop = true;
        altSource.Play();

        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            altSource.volume = Mathf.Lerp(0f, targetVol, timer/duration);
            yield return null;
        }
        altSource.volume = targetVol;
    }

    void FadeInAudio()
    {
        StartCoroutine(FadeInCoroutine(ambientTrack, 0.7f, 5));
    }

    public void Start()
    {
        FadeInAudio();
       
    }
}
