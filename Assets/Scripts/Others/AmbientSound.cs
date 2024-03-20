using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public static AmbientSound Instance { get; private set; }

    [SerializeField] private AudioClip ambient1;
    [SerializeField] private AudioClip ambient2;

    private AudioSource _audio;

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

    public void StopAll()
    {
        _audio.Stop();
    }

    public void ContinuePlaying()
    {
        if (!Globals.IsMusicOn) return;

        PlaySound((AmbientSounds)UnityEngine.Random.Range(0,2));
    }

    public void PlaySound(AmbientSounds sound)
    {
        if (!Globals.IsMusicOn) return;

        _audio.volume = 0.5f;
        _audio.loop = true;

        switch (sound)
        {
            case AmbientSounds.ambient1:
                _audio.clip = ambient1;
                _audio.Play();
                break;

            case AmbientSounds.ambient2:
                _audio.clip = ambient2;
                _audio.Play();
                break;
        }
    }
}

public enum AmbientSounds
{
    ambient1,
    ambient2
}
