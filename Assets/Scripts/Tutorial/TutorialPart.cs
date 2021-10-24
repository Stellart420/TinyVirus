using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirusGame;

public class TutorialPart : MonoBehaviour
{
    [SerializeField] public TutorialPartType partType;
    [SerializeField] float waitTime = 0;

    Action PartComplete;
    bool checking = false;
    CanvasGroup canvasGroup;
    TutorialVirus tutorVirus;

    public TutorialVirus VirusTutorial => tutorVirus;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Activate(Action part_complete)
    {
        PartComplete += part_complete;
        canvasGroup.LeanAlpha(1, 1f).setOnComplete(() =>
        {
            switch (partType)
            {
                case TutorialPartType.Wait:
                    StartCheckWaitTime();
                    break;
                case TutorialPartType.Drag:
                    CheckDrag();
                    break;
                case TutorialPartType.Tap:
                    CheckTap();
                    break;
                case TutorialPartType.DoubleTap:
                    CheckDoubleTap();
                    break;
                default: break;

            }
        });
    }

    public void Deactivate(Action end_deactivate_action = null)
    {
        canvasGroup.LeanAlpha(0, 0.5f).setOnComplete(() => end_deactivate_action?.Invoke());
    }

    private void Update()
    {

    }

    void StartCheckWaitTime()
    {
        checking = true;
        StartCoroutine(CheckWaitTime());
    }

    IEnumerator CheckWaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        checking = false;
        PartComplete?.Invoke();
    }

    void CheckTap()
    {
        TapController.instance.Interaction += CheckInteraction;
    }

    void CheckDrag()
    {
        LevelController.instance.CurrentLevel.Viruses.ForEach(virus =>
        {
            var tutor_virus = virus.GetComponent<TutorialVirus>();
            if (tutor_virus != null)
            {
                tutorVirus = tutor_virus;
                tutorVirus.Draged += (drag) => CheckInteraction(InteractionType.Drag, drag);
                tutorVirus.Activate();
            }
        });
    }

    void CheckDoubleTap()
    {
        LevelController.instance.CurrentLevel.Viruses.ForEach(virus =>
        {
            var tutor_virus = virus.GetComponent<TutorialVirus>();
            if (tutor_virus != null)
            {
                tutorVirus = tutor_virus;
                tutorVirus.DoubleTaped += (double_taped) => CheckInteraction(InteractionType.DoubleTap, double_taped);
                tutorVirus.Activate();
            }
        });
    }

    void CheckInteraction(Vector2 tap_pos, InteractionType interaction_type, Collider2D col)
    {
        if (interaction_type == InteractionType.Tap)
            CheckInteraction(InteractionType.Tap);
    }

    void CheckInteraction(InteractionType interaction, DragAndDrop drag = null)
    {
        switch (interaction)
        {
            case InteractionType.Tap:
                if (partType == TutorialPartType.Tap)
                {
                    checking = false;

                    TapController.instance.Interaction -= CheckInteraction;
                    PartComplete?.Invoke();
                }
                break;
            case InteractionType.Drag:
                if (partType == TutorialPartType.Drag)
                {
                    tutorVirus.DeActivate();
                    tutorVirus.Draged -= (dr) => CheckInteraction(InteractionType.Drag, dr);
                    drag.DeactiveMoved();
                    tutorVirus = null;
                    checking = false;
                    PartComplete?.Invoke();
                }
                break;
            case InteractionType.DoubleTap:
                if (partType == TutorialPartType.DoubleTap)
                {
                    tutorVirus.DeActivate();
                    tutorVirus.DoubleTaped -= (dr) => CheckInteraction(InteractionType.DoubleTap, dr);
                    drag.DeactiveMoved();
                    tutorVirus = null;
                    checking = false;
                    PartComplete?.Invoke();
                }
                break;
            default: break;
        }
    }
}

public enum TutorialPartType
{
    Wait,
    Tap,
    Hold,
    Drag,
    DoubleTap,
}