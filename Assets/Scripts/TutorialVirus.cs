using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirusGame;

public class TutorialVirus : Virus
{
    [SerializeField] GameObject Finger;

    public Action<DragAndDrop> Draged;
    public Action<DragAndDrop> DoubleTaped;

    public void Activate()
    {
        Finger.SetActive(true);
        GetComponent<DragAndDrop>().Interaction += CheckInteraction;
    }
    
    void CheckInteraction(InteractionType interaction_type)
    {
        if (interaction_type == InteractionType.Drag)
            Draged?.Invoke(GetComponent<DragAndDrop>());

        if (interaction_type == InteractionType.DoubleTap)
            DoubleTaped?.Invoke(GetComponent<DragAndDrop>());
    }


    public void DeActivate()
    {
        Finger.SetActive(false);
        GetComponent<DragAndDrop>().Interaction -= CheckInteraction;
    }
}
