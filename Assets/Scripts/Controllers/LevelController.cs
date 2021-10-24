using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public GameObject levelContainer;
    
    Level currentLevel;

    public Level CurrentLevel => currentLevel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        
    }

    private void Start()
    {
        currentLevel = levelContainer.GetComponentInChildren<Level>();

        if (currentLevel != null)
            LoadLevel(currentLevel.Index);
        else
        {
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        LoadLevel(DataManager.instance.LoadLvl);
    }

    public void LoadLevel(int index)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }

        currentLevel = Instantiate(DataManager.instance.GetLevel(index), levelContainer.transform);
        Camera.main.orthographicSize = currentLevel.CameraSize;
        Debug.Log($"Уровень: {index}");

        TutorialController.instance.CheckTutorial();
    }
}
