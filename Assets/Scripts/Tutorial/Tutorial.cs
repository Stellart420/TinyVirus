using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Tutorial : MonoBehaviour
{
    [SerializeField] List<TutorialPart> tutorialParts;


    int current_part_index = 0;
    bool tutorial_enabled = false;
    TutorialPart current_part;

    public TutorialPart CurrentPart => current_part;
    public void Activate()
    {
        TutorialController.instance.tutorialActivated = true;
        ActivatePart(0);
    }

    void ActivatePart(int index)
    {
        if (index >= tutorialParts.Count)
        {
            Debug.LogError($"Нет такого элемента тутора");
            return;
        }

        if (current_part == null)
        {
            current_part = tutorialParts[index];
            current_part_index = index;
            current_part.Activate(ActivateNext);
        }
        else
        {
            current_part.Deactivate(() =>
            {
                current_part = tutorialParts[index];
                current_part_index = index;
                current_part.Activate(ActivateNext);
            });
        }
    }

    void ActivateNext()
    {
        if (current_part_index + 1 < tutorialParts.Count)
        {
            ActivatePart(current_part_index + 1);
        }
        else
        {
            if (current_part != null)
            {
                current_part.Deactivate(() =>
                {
                    current_part = null;
                    EndTutorial();
                });
            }
        }
    }

    void EndTutorial()
    {
        var game_window = UIController.Instance.Get<GameWindow>() as GameWindow; 
        game_window.ShowTask();
        TutorialController.instance.tutorialActivated = false;
        Destroy(gameObject);
    }
}
