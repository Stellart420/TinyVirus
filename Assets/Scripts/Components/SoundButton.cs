using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [Header("Sound")]
    Button soundBtn;
    [SerializeField] GameObject soundOn;
    [SerializeField] GameObject soundOff;
    public void Start()
    {
        soundBtn = GetComponent<Button>();
        if (soundBtn != null) soundBtn.onClick.AddListener(() => ClickSound());

        SetSound(MusicController.Instance.SoundIsActive);
    }

    private void OnEnable()
    {
        if (MusicController.Instance != null)
            SetSound(MusicController.Instance.SoundIsActive);
    }

    public void ClickSound()
    {
        MusicController.Instance.ActiveSound();
        SetSound(MusicController.Instance.SoundIsActive);
    }

    public void SetSound(bool value)
    {
        soundOn.SetActive(value);
        soundOff.SetActive(!value);
    }
}
