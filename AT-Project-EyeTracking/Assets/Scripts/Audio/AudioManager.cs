using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("General Audio Sources")]
    public AudioSource uiSource;

    public AudioSource altSource;

    public AudioClip slotHover;

    public AudioClip selectedItem;

    public AudioClip pickUpAudio;

    public AudioClip dropItem;

    public AudioClip ambientTrack;

    public AudioClip paperSelect;

    [Header("SCP 173 Audio Sources and Clips")]

    public AudioSource SCP173Audio;

    public AudioClip seen173Clip1;

    public AudioClip seen173Clip2;

    public AudioClip neckSnap;

    public AudioClip scp173Jumpscare;

    [Header("SCP 689 Audio Sources and Clips")]

    public AudioSource scp689Audio;

    public AudioClip spawn689;

    public AudioClip despawn689;

    [Header("SCP 096 Audio Sources and Clips")]

    public AudioSource scp096Audio;

    public AudioClip chase096;

    public AudioClip scp096Slash;

    public AudioClip scp096Slash2;

    [Header("Player FootSteps Audio Source")]

    public AudioSource playerFootsteps;

    public AudioSource playerAlt;

    public AudioClip heartBeatSlow;

    public AudioClip heartBeatFast;

    public AudioClip walkingSteps;

    public AudioClip sprintingSteps;


    public void HoveringItem()
    {
        uiSource.volume = 0.5f;
        uiSource.PlayOneShot(slotHover);
    }
    public void SelectedItem()
    {
        uiSource.PlayOneShot(selectedItem);
    }

    public void PaperSelect()
    {
        uiSource.PlayOneShot(paperSelect);
    }

    public void DropItem()
    {
        uiSource.PlayOneShot(dropItem);
    }
    public void PlayAmbientTrack()
    {
        PlayLoopedAudio(altSource, ambientTrack, 0.7f);
    }
    public void Play096Chase()
    {
        PlayLoopedAudio(altSource, chase096, 0.6f);
    }

    public void PlayPickUpAudio()
    {
        uiSource.PlayOneShot(pickUpAudio);
    }

    public void Play689Spawn()
    {
        scp689Audio.PlayOneShot(spawn689);
        scp689Audio.volume = 0.4f;
    }

    public void Play689Despawn()
    {
        scp689Audio.PlayOneShot(despawn689);
        scp689Audio.volume = 0.4f;
    }

    public void PlayLoopedAudio(AudioSource source, AudioClip clip, float volume = 1f)
    {
        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        source.Play();
    }

    public void PlaySCP173Audio()
    {
        List<AudioClip> randomAudio = new();

        randomAudio.Add(seen173Clip1);
        randomAudio.Add(seen173Clip1);

        var temp = seen173Clip1;

        for (int i = 0; i < randomAudio.Count; i++)
        {
            AudioClip audioClip = randomAudio[i];
            temp = randomAudio[i];
        }

        SCP173Audio.volume = 0.6F;
        SCP173Audio.clip = temp;
        SCP173Audio.Play();
    }

    public void PlayNeckSnap()
    {
        SCP173Audio.volume = 0.6F;
        SCP173Audio.clip = neckSnap;
        SCP173Audio.Play();
    }

    public void PlaySCP096Slash()
    {
        scp096Audio.volume = 0.6F;
        scp096Audio.clip = scp096Slash;
        scp096Audio.Play();
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
