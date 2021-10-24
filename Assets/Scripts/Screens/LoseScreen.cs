using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : Window
{
    [SerializeField] Button restartBtn;
    [SerializeField] Button menuBtn;
    Animator animator;
    private void Awake()
    {
        if (restartBtn != null) restartBtn.onClick.AddListener(() => RestartClick());
        if (menuBtn != null) menuBtn.onClick.AddListener(() => MenuClick());
        animator = GetComponent<Animator>();
    }

    public void RestartClick()
    {
        GameController.Instance.Restart();
    }

    public void MenuClick()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ShowAd()
    {
        ADController.instance.ShowInterstitial();
    }
    protected override void SelfOpen()
    {
        MusicController.Instance.ControlMusic(false);
        MMVibrationManager.Haptic(HapticTypes.Failure);
        GetComponent<AudioSource>().Play();
        gameObject.SetActive(true);
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetTrigger("Show");
    }

    protected override void SelfClose()
    {
        animator.SetTrigger("Hide");
        MusicController.Instance.ControlMusic(true);
        gameObject.SetActive(false);
    }
}
