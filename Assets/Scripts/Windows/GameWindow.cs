using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : Window
{
    [SerializeField] Text time;
    [SerializeField] Slider healthBar;
    [SerializeField] CanvasGroup taskPanel;
    [SerializeField] Text taskText;

    [SerializeField] Button pauseBtn;
    [SerializeField] LocalizedString hold_task;
    [SerializeField] LocalizedString destroy_task;

    private void Awake()
    {
        if (pauseBtn != null) pauseBtn.onClick.AddListener(() => UIController.Instance.Get<PauseWindow>().Open());
        GameController.Instance.GameStateChanged += (state) =>
        {
            if (state == GameState.Pause)
            {
                pauseBtn.gameObject.SetActive(false);
                return;
            }
            pauseBtn.gameObject.SetActive(true);
        };
    }

    public void SetMaxHealth(int value)
    {
        healthBar.maxValue = value;
        healthBar.value = value;
    }

    public void ChangeHealth(int value)
    {
        healthBar.value = value;
    }

    private void Start()
    {
        GameController.Instance.TimeChanged += ChangeTime;
        GameController.Instance.ResetTime();
        if (!TutorialController.instance.tutorialActivated)
            ShowTask();

        if (LevelController.instance.CurrentLevel.Type == LevelType.Destroyed)
        {
            time.transform.parent.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            time.transform.parent.gameObject.SetActive(true);
            healthBar.gameObject.SetActive(false);
        }
    }

    public void ShowTask()
    {
        var level = LevelController.instance.CurrentLevel;
        string task_text = "";
        if (level.Type == LevelType.Hold)
            task_text = hold_task; //$"Hold On {LevelController.instance.CurrentLevel.WinTime} seconds";
        else
            task_text = destroy_task; //"Destroy The Boss";

        taskText.text = task_text;
        taskPanel.LeanAlpha(1, 1f).setOnComplete(()=>
        {
            taskPanel.LeanAlpha(0, 1f).setDelay(3f).setOnComplete(() => GameController.Instance.GameStart());
        });
    }

    void ChangeTime(int value)
    {
        time.text = $"{value}";
    }

    protected override void SelfClose()
    {
    }

    protected override void SelfOpen()
    {
    }
}
