using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : Window
{
    [SerializeField] Button nextBtn;
    [SerializeField] Button menuBtn;
    Animator animator;
    private void Awake()
    {
        if (nextBtn != null) nextBtn.onClick.AddListener(() => NextClick());
        if (menuBtn != null) menuBtn.onClick.AddListener(() => MenuClick());
        animator = GetComponent<Animator>();
    }

    public void NextClick()
    {
        GameController.Instance.Next();
    }

    public void MenuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    //protected override void SelfOpen()
    //{
    //    var rect = GetComponent<RectTransform>();
    //    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 1000);
    //    rect.LeanMoveLocalY(0, 1f);
    //    MusicController.Instance.ControlMusic(false);
    //    GetComponent<AudioSource>().Play();
    //    bool next_enabled = DataManager.instance.LoadLvl + 1 <= DataManager.instance.LevelsCount;
    //    var canvas_group = GetComponent<CanvasGroup>();
    //    canvas_group.LeanAlpha(1, 0.5f).setDelay(0.5f).setOnStart(() =>
    //    {
    //        gameObject.SetActive(true);
    //    }).setOnComplete(() =>
    //    {
    //        nextBtn.GetComponent<RectTransform>().LeanAlpha(next_enabled ? 1 : 0, 1f);
    //    });
    //}

    public void ShowAd()
    {
        ADController.instance.ShowInterstitial();
    }

    protected override void SelfOpen()
    {
        MusicController.Instance.ControlMusic(false);
        MMVibrationManager.Haptic(HapticTypes.Success);
        GetComponent<AudioSource>().Play();
        gameObject.SetActive(true);
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetTrigger("Show");
        bool next_enabled = DataManager.instance.LoadLvl + 1 <= DataManager.instance.LevelsCount;
        nextBtn.GetComponent<RectTransform>().LeanAlpha(next_enabled ? 1 : 0, 1f);
    }

    protected override void SelfClose()
    {
        animator.SetTrigger("Hide");

        MusicController.Instance.ControlMusic(true);
        gameObject.SetActive(false);
    }
}
