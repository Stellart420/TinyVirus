using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance = null;

    public RectTransform fade;
    public List<GameObject> Windows;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        InitUI();
    }

    private void InitUI()
    {
    }

    public Window Get<T>() where T : Window
    {
        foreach (var window in Windows)
        {
            var windowComponent = window.GetComponent<Window>();
            if (windowComponent is T)
                return windowComponent;
        }
        return null;
    }
}
