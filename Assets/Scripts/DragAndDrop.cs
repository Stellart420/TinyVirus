using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirusGame
{
    public class DragAndDrop : MonoBehaviour
    {
        [Header("Эффекты")]
        [SerializeField] GameObject selectionEffect;

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
            if (col == null)
                col = GetComponentInChildren<PolygonCollider2D>();
            TapController.instance.Interaction += CheckInteraction;
        }

        void CheckInteraction(Vector2 touch_pos, InteractionType interaction_type, Collider2D touched_col)
        {
            if (GameController.Instance.GameState == GameState.Pause && !TutorialController.instance.tutorialActivated)
                return;


            //print($"n:{name}");

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
                    CheckHold(touch_pos, touched_col);
                    break;
                case InteractionType.Up:
                    if (touched_col == col)
                        moveAllowed = false;
                    break;
                case InteractionType.Drag:
                    CheckDrag(touched_col);
                    break;
                default: CheckTap(touched_col); break;
            }
        }

        void CheckTap(Collider2D touched_col)
        {
            if (touched_col == null)
                return;

            if (touched_col == col)
            {
                Debug.Log($"Tap: {touched_col.name}");
                MMVibrationManager.Vibrate();//.Haptic(HapticTypes);
                moveAllowed = false;
                Instantiate(selectionEffect, transform.position, Quaternion.identity);
                if (MusicController.Instance.SoundIsActive) source.Play();

                var vir = col.GetComponent<Virus>();

                if (vir == null)
                    vir = col.GetComponentInParent<Virus>();

                if (vir.CanMove)
                    moveAllowed = true;

                if ((lastClick + interval) > Time.time)
                {
                    Interaction?.Invoke(InteractionType.DoubleTap);
                    if (vir.type == VirusType.Boss10 || vir.type == VirusType.Boss20)
                    {
                        var boss_virus = vir as BossVirus;
                        if (boss_virus != null)
                            boss_virus.Damaged();

                        var boss_virus_shoot = vir as VirusShoot;
                        if (boss_virus_shoot != null)
                            boss_virus_shoot.Damaged();
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

        void CheckHold(Vector2 hold_pos, Collider2D touched_col)
        {
            if (moveAllowed && touched_col == col)
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

        private void OnDestroy()
        {
            TapController.instance.Interaction -= CheckInteraction;
        }
    }
}