using I2.Loc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] public GameObject BG;
    [SerializeField] Text number;
    [SerializeField] GameObject locker;
    [SerializeField] bool isLocked = true;
    [Header("Цвета")]
    [SerializeField] Color lockedColor;
    [SerializeField] Color unlockedColor;

    [SerializeField] LocalizedString level;

    public void Init(int index, UnityAction click_action)
    {
        isLocked = DataManager.instance.LastLvl < index;
        //number.color = isLocked ? lockedColor : unlockedColor;
        number.text = level/*.mTerm.ToUpper()*/ + $" {index}";
        locker.SetActive(isLocked);

        if (!isLocked)
            GetComponent<Button>().onClick.AddListener(() =>
            {
                LoadLevel(index);
                click_action();
            });
    }

    void LoadLevel(int lvl_index)
    {
        MusicController.Instance.PlaySelectSound();
        if (DataManager.instance.LastLvl < lvl_index)
            return;

        DataManager.instance.SetCurrentLevel(lvl_index);
    }
}