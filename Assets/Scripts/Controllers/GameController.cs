using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Крайние точки")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("UI")]
    [SerializeField] Virus virus;

    public delegate void TimeEventHandler(int time);
    public event TimeEventHandler TimeChanged;

    public delegate void GameStateEventHandler(GameState state);
    public event GameStateEventHandler GameStateChanged;

    int time;

    Level level;
    public int AddTime(int value)
    {
        return SetTime(time + value);
    }

    public int SetTime(int value)
    {
        time = value;
        if (TimeChanged != null)
            TimeChanged(time);

        return time;
    }

    public void ResetTime()
    {
        if (level.Type == LevelType.Hold)
            timeForWin = LevelController.instance.CurrentLevel.WinTime;

        SetTime(timeForWin);
    }

    private GameState gameState = GameState.Pause;

    public GameState GameState => gameState;

    public void ChangeGameState(GameState state)
    {
        gameState = state;
        GameStateChanged?.Invoke(gameState);
    }

    int timeForWin;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxX = stageDimensions.x - virus.Size.x / 2;
        minX = -maxX;
        maxY = stageDimensions.y - virus.Size.y / 2;
        minY = -maxY;
        level = LevelController.instance.CurrentLevel;
        if (level.Type == LevelType.Hold)
        {
            ResetTime();
            StartCoroutine(ScoreTimer());
        }
    }

    private void Update()
    {
    }

    public void GameStart()
    {
        ResetTime();
        ChangeGameState(GameState.Play);
    }

    IEnumerator ScoreTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            if (gameState != GameState.Play)
                continue;

            AddTime(-1);

            if (time <= 0)
            {
                Win();
            }
        }
    }

    public void GameOver()
    {
        ChangeGameState(GameState.Pause);
        UIController.Instance.Get<LoseScreen>().Open();

    }

    public void Win()
    {
        DataManager.instance.UnlockNext();
        ChangeGameState(GameState.Pause);
        UIController.Instance.Get<WinScreen>().Open();
    }

    public void Next()
    {
        DataManager.instance.SetNextLevel();
        Restart();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState
{
    Play,
    Pause,
}
