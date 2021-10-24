using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    [SerializeField] AudioSource source;

    [SerializeField] AudioClip selectClip;
    public bool MusicIsActive
    {
        get
        {
            return PlayerPrefs.GetInt("Music", 1) == 1;
        }
        private set
        {
            int checker = value ? 1 : 0;
            PlayerPrefs.SetInt("Music", checker);
        }
    }

    public bool SoundIsActive
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        private set
        {
            int checker = value ? 1 : 0;
            PlayerPrefs.SetInt("Sound", checker);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        source.volume = MusicIsActive ? 0.5f : 0f;
    }

    public void PlaySelectSound()
    {
        var aud = gameObject.AddComponent<AudioSource>();
        aud.PlayOneShot(selectClip, SoundIsActive ? 1 : 0);
        MMVibrationManager.Haptic(HapticTypes.Selection);
    }
    public void ActiveMusic()
    {
        MusicIsActive = !MusicIsActive;

        source.time = 0;
        source.volume = MusicIsActive ? 0.5f : 0f;
    }

    public void ActiveSound()
    {
        SoundIsActive = !SoundIsActive;
    }

    public void ControlMusic(bool is_active)
    {
        if (is_active)
            is_active = SoundIsActive;

        source.volume = is_active ? 1 : 0;
    }
}
