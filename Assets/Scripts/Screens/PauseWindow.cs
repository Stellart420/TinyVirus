using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindow : Window
{
    [SerializeField] Button continueBtn;
    [SerializeField] Button menuBtn;
    Animator animator;
    private void Awake()
    {
        if (continueBtn != null) continueBtn.onClick.AddListener(() => ContinueClick());
        if (menuBtn != null) menuBtn.onClick.AddListener(() => MenuClick());
        animator = GetComponent<Animator>();
    }

    public void ContinueClick()
    {
        Close();
    }

    public void MenuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    protected override void SelfClose()
    {
        //var canvas_group = GetComponent<CanvasGroup>();
        //canvas_group.LeanAlpha(0, 0f).setOnComplete(() =>
        //{
        //    gameObject.SetActive(false);
        //    if (!TutorialController.instance.tutorialActivated)
        //        GameController.Instance.ChangeGameState(GameState.Play);
        //});
        animator.SetBool("isShown", false);
    }

    protected override void SelfOpen()
    {
        gameObject.SetActive(true);

        animator.SetBool("isShown", true);
        GameController.Instance.ChangeGameState(GameState.Pause);
        //var canvas_group = GetComponent<CanvasGroup>();
        //canvas_group.LeanAlpha(1, 0.5f).setDelay(0.5f).setOnStart(() =>
        //{
        //    gameObject.SetActive(true);
        //    GameController.Instance.ChangeGameState(GameState.Pause);
        //});
    }

    public void Hide()
    {
        if (!TutorialController.instance.tutorialActivated)
            GameController.Instance.ChangeGameState(GameState.Play);
        animator.SetBool("isShown", false);
        gameObject.SetActive(false);
    }
}
