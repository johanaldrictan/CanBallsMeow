using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAudio : MonoBehaviour
{
    public AudioClip CatDeath;
    public AudioClip CatEat;
    public AudioClip[] CatMeows;
    public AudioClip CatAttack;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCatDeath()
    {
        audioSource.clip = CatDeath;
        audioSource.pitch = 1;
        audioSource.Play();
    }

    public void PlayCatEat()
    {
        audioSource.clip = CatEat;
        audioSource.pitch = Random.Range(.8f, 1.3f);
        audioSource.Play();
    }

    public void PlayCatMeow()
    {
        audioSource.clip = CatMeows[Random.Range(0,2)];
        audioSource.pitch = Random.Range(.8f, 1.3f);
        audioSource.Play();
    }

    public void PlayCatAttack()
    {
        audioSource.clip = CatAttack;
        audioSource.pitch = Random.Range(.8f, 1.3f);
        audioSource.Play();
    }
}
