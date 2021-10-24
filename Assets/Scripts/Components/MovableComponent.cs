using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableComponent : MonoBehaviour
{
    public GameObject selectionEffect;

    bool moveAllowed;

    float lastClick = 0f;
    float interval = 0.4f;

    Collider2D col;
    AudioSource source;
    Virus virus;
    public Action<InteractionType> Interaction;

    public void DeactiveMoved()
    {
        moveAllowed = false;
    }
    private void Start()
    {
        virus = GetComponent<Virus>();
        col = GetComponent<Collider2D>();
        source = GetComponent<AudioSource>();
        TapController.instance.Interaction += CheckInteraction;
    }

    void CheckInteraction(Vector2 touch_pos, InteractionType interaction_type, Collider2D touched_col)
    {
        if (this == null)
            return;

        if (GameController.Instance.GameState == GameState.Pause && !TutorialController.instance.tutorialActivated)
            return;

        if (TutorialController.instance.CurrentTutorial != null)
        {
            if (virus as TutorialVirus == null || virus as TutorialVirus != TutorialController.instance.CurrentTutorial.CurrentPart.VirusTutorial)
                return;
        }

        switch (interaction_type)
        {
            case InteractionType.Tap:
                CheckTap(touched_col);
                break;
            case InteractionType.Hold:
                CheckHold(touch_pos);
                break;
            case InteractionType.Up:
                moveAllowed = false;
                break;
            case InteractionType.Drag:
                CheckDrag(touched_col);
                break;
            default: break;
        }
    }

    void CheckTap(Collider2D touched_col)
    {
        if (touched_col == null)
            return;

        if (touched_col == col)
        {
            Debug.Log($"Tap: {touched_col.name}");
            moveAllowed = false;
            Instantiate(selectionEffect, transform.position, Quaternion.identity);
            source.Play();

            var vir = col.GetComponent<Virus>();
            if (vir.CanMove)
                moveAllowed = true;

            var boss_virus = vir as BossVirus;

            if ((lastClick + interval) > Time.time)
            {
                Interaction?.Invoke(InteractionType.DoubleTap);
                if (boss_virus != null)
                {
                    boss_virus.Damaged();
                }

                if (vir.IsDestroyable)
                {
                    vir.Destroyed();
                }
            }
            else
            {
                Interaction?.Invoke(InteractionType.Tap);
            }

            lastClick = Time.time;
        }
    }

    void CheckHold(Vector2 hold_pos)
    {
        if (moveAllowed && TapController.instance.TouchedCollider == col)
        {

            transform.position = new Vector2(Mathf.Clamp(hold_pos.x, GameController.Instance.minX, GameController.Instance.maxX), Mathf.Clamp(hold_pos.y, GameController.Instance.minY, GameController.Instance.maxY));
            Interaction?.Invoke(InteractionType.Hold);
        }
    }

    void CheckDrag(Collider2D touch_col)
    {
        if (moveAllowed && touch_col == col)
        {

            Interaction?.Invoke(InteractionType.Drag);
        }
    }
}
