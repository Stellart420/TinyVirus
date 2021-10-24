using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuWindow : Window
{
    [SerializeField] Button playBtn;
    private void Awake()
    {
        playBtn.onClick.AddListener(() => PlayBtnClick());
    }

    void PlayBtnClick()
    {
        MusicController.Instance.PlaySelectSound();
        DataManager.instance.SetCurrentLevel(DataManager.instance.LastLvl);
        UIController.Instance.fade.LeanAlpha(1,1f).setOnComplete(()=>GoToGameScene());
    }
    void GoToGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LevelsButtonLick()
    {
        MusicController.Instance.PlaySelectSound();
        UIController.Instance.fade.LeanAlpha(1, 0.5f).setOnComplete(() =>
        {
            UIController.Instance.Get<LevelScreen>().Open();
            UIController.Instance.fade.LeanAlpha(0, 0.5f);
        }).setOnStart(()=>
        {
            Close();
        });
    }

    protected override void SelfClose()
    {
        gameObject.SetActive(false);
    }

    protected override void SelfOpen()
    {
        gameObject.SetActive(true);
    }
}
