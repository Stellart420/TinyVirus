using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibroButton : MonoBehaviour
{
    [Header("Vibro")]
    Button vibroBtn;
    [SerializeField] GameObject VibroOn;
    [SerializeField] GameObject VibroOff;
    Animator animator;
    public void Start()
    {
        vibroBtn = GetComponent<Button>();
        animator = GetComponent<Animator>();
        if (vibroBtn != null) vibroBtn.onClick.AddListener(() => ClickVibro());
        SetVibro(VibroIsActive);
    }

    private void OnEnable()
    {
        SetVibro(VibroIsActive);
    }
    public void ClickVibro()
    {
        MMVibrationManager.SetHapticsActive(!VibroIsActive);
        VibroIsActive = !VibroIsActive;
        SetVibro(VibroIsActive);
    }

    bool VibroIsActive
    {
        get
        {
            return PlayerPrefs.GetInt("Vibro", 1) == 1;
        }
        set
        {

            PlayerPrefs.SetInt("Vibro", value ? 1 : 0);
        }
    }
    public void SetVibro(bool value)
    {
        VibroOn.SetActive(value);
        VibroOff.SetActive(!value);
    }
}
