using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public static AmbientSound Instance { get; private set; }

    [SerializeField] private AudioClip ambient1;

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

    }

    public void ContinuePlaying()
    {

    }

    public void PlaySound(AmbientSounds sound)
    {
        _audio.volume = 0.5f;
        _audio.loop = true;

        switch (sound)
        {
            case AmbientSounds.ambient1:
                _audio.clip = ambient1;
                _audio.Play();
                break;
        }
    }
}

public enum AmbientSounds
{
    ambient1,
}
