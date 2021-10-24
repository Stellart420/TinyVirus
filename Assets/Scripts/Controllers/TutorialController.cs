using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;

    [SerializeField] Transform tutorialParent;

    public bool tutorialActivated = false;

    Tutorial currentTutorial;

    public Tutorial CurrentTutorial => currentTutorial;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public bool CheckTutorial()
    {
        if (LevelController.instance.CurrentLevel.IsTutorial)
        {
            GameController.Instance.ChangeGameState(GameState.Pause);
            currentTutorial = Instantiate(LevelController.instance.CurrentLevel.Tutorial, tutorialParent);
            currentTutorial.Activate();
            return true;
        }

        return false;

    }
}
