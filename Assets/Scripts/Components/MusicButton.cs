using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [Header("Music")]
    Button musicBtn;
    [SerializeField] GameObject MusicOn;
    [SerializeField] GameObject MusicOff;
    Animator animator;
    public void Start()
    {
        musicBtn = GetComponent<Button>();
        animator = GetComponent<Animator>();
        if (musicBtn != null) musicBtn.onClick.AddListener(() => ClickMusic());
        SetMusic(MusicController.Instance.MusicIsActive);
    }

    private void OnEnable()
    {
        if (MusicController.Instance != null)
            SetMusic(MusicController.Instance.MusicIsActive);
    }
    public void ClickMusic()
    {
        MusicController.Instance.ActiveMusic();
        SetMusic(MusicController.Instance.MusicIsActive);
    }
    public void SetMusic(bool value)
    {
        MusicOn.SetActive(value);
        MusicOff.SetActive(!value);
    }
}
