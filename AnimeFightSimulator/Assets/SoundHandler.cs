using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip whoosh;
    public AudioClip punch;

    public void PlayWhoosh()
    {
        audioSource.PlayOneShot(whoosh);
    }

    public void PlayPunch()
    {
        audioSource.PlayOneShot(punch);
    }
}
