using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager instance;
    [SerializeField] List<Level> levels;

    public Level GetLevel(int index)
    {
        if (levels.Count < index)
            return levels[levels.Count];

        return levels[index-1];
    }

    public int LevelsCount => levels.Count;

    int lastLvl
    {
        get
        {
            return PlayerPrefs.GetInt("LastLevel", 1);
        }
        set
        {
            if (value > levels.Count)
                return;

            if (lastLvl > value)
                return;

            PlayerPrefs.SetInt("LastLevel", value);
        }
    }

    int loadLvl;

    public int LoadLvl => loadLvl;

    public int LastLvl => lastLvl;

    public void SetNextLevel()
    {
        SetCurrentLevel(loadLvl+1);
    }
    public void SetCurrentLevel(int value)
    {
        if (value > levels.Count)
            return;

        if (lastLvl < value)
            lastLvl = value;

        Debug.Log($"Load:{value}");
        loadLvl = value;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        levels.AddRange(Resources.LoadAll<Level>("Levels").OrderBy(level => level.Index));
        if (Debug.isDebugBuild)
            PlayerPrefs.SetInt("LastLevel", levels.Count);
        lastLvl = PlayerPrefs.GetInt("LastLevel", 1);
        loadLvl = lastLvl;
    }

    public void UnlockNext()
    {
        if (lastLvl < loadLvl + 1)
            lastLvl = loadLvl + 1;
    }

}
