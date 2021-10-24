using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScreen : Window
{
    [SerializeField] LevelButton levelBtn;
    [SerializeField] GameObject levelBtnsContainer;
    [SerializeField] List<LevelButton> levelBtns;

    private void Awake()
    {
    }

    private void Start()
    {
        var levels_count = Resources.LoadAll<Level>("Levels").Length;
        for (int i = 0; i < levels_count;i++)
        {
            var level_btn = Instantiate(levelBtn, levelBtnsContainer.transform);
            levelBtns.Add(level_btn);
        }
        for (int i = 0; i < levelBtns.Count; i++)
        {
            levelBtns[i].Init(i + 1, () => GoToGameScene());
        }
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackBtnClick()
    {
        MusicController.Instance.PlaySelectSound();
        Close();
    }

    protected override void SelfOpen()
    {
        gameObject.SetActive(true);
    }

    protected override void SelfClose()
    {
        UIController.Instance.fade.LeanAlpha(1, 0.5f).setOnComplete(() =>
        {
            gameObject.SetActive(false);
            UIController.Instance.fade.LeanAlpha(0, 0.5f);
            UIController.Instance.Get<MenuWindow>().Open();
        });
    }
}
