using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public static UISound Instance { get; private set; }

    [SerializeField] private AudioClip hit1;
    [SerializeField] private AudioClip hit2;
    [SerializeField] private AudioClip hit3;
    [SerializeField] private AudioClip hit4;
    [SerializeField] private AudioClip hit5;
    [SerializeField] private AudioClip hit6;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip startLevel;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip stretching;
    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioClip lowImpact;

    private AudioSource _audio;
    private int lastRandomForHit;
    private float hitVolume = 0.2f;
    private float hitSpeed = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _audio = GetComponent<AudioSource>();
    }

    public void PlayAnyHit()
    {
        int rnd = 0;

        for (int i = 0; i < 5; i++)
        {
            rnd = UnityEngine.Random.Range(0, 6);

            if (rnd != lastRandomForHit)
            {
                lastRandomForHit = rnd;
                break;
            }
        }
                
        PlaySound((SoundsUI)rnd);
    }

    public void PlaySound(SoundsUI sound)
    {
        _audio.volume = 0.3f;
        _audio.pitch = 1;

        switch (sound)
        {
            case SoundsUI.hit1:
                _audio.volume = hitVolume;
                _audio.pitch = hitSpeed;
                _audio.clip = hit1;
                _audio.Play();
                break;

            case SoundsUI.hit2:
                _audio.volume = hitVolume;
                _audio.pitch = hitSpeed;
                _audio.clip = hit2;
                _audio.Play();
                break;

            case SoundsUI.hit3:
                _audio.volume = hitVolume;
                _audio.pitch = hitSpeed;
                _audio.clip = hit3;
                _audio.Play();
                break;

            case SoundsUI.hit4:
                _audio.volume = hitVolume;
                _audio.pitch = hitSpeed;
                _audio.clip = hit4;
                _audio.Play();
                break;

            case SoundsUI.hit5:
                _audio.volume = hitVolume;
                _audio.pitch = hitSpeed;
                _audio.clip = hit5;
                _audio.Play();
                break;

            case SoundsUI.hit6:
                _audio.volume = hitVolume;
                _audio.pitch = hitSpeed;
                _audio.clip = hit6;
                _audio.Play();
                break;

            case SoundsUI.win:
                _audio.clip = win;                
                _audio.Play();
                break;

            case SoundsUI.lose:
                _audio.volume = 0.4f;
                _audio.clip = lose;
                _audio.Play();
                break;

            case SoundsUI.start:
                _audio.clip = startLevel;
                _audio.volume = 0.2f;
                _audio.pitch = 1.2f;
                _audio.Play();
                break;

            case SoundsUI.error:
                _audio.clip = error;
                _audio.Play();
                break;

            case SoundsUI.pop:
                _audio.clip = pop;
                _audio.Play();
                break;

            case SoundsUI.click:
                _audio.clip = click;
                _audio.Play();
                break;

            case SoundsUI.stretching:
                _audio.clip = stretching;
                _audio.Play();
                break;

            case SoundsUI.impact:
                _audio.clip = impact;
                _audio.volume = 0.9f;
                _audio.Play();
                break;

            case SoundsUI.low_impact:
                _audio.clip = lowImpact;
                _audio.volume = 0.5f;
                _audio.Play();
                break;
        }
    }
}

public enum SoundsUI
{
    hit1,
    hit2, 
    hit3, 
    hit4, 
    hit5,
    hit6,
    lose,
    win,
    start,
    error,
    pop,
    click,
    stretching,
    impact,
    low_impact

}