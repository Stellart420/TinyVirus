using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAssistant : MonoBehaviour
{
    Text messageText;
    bool enabled = false;
    private void Awake()
    {
        messageText = GetComponent<Text>();
    }

    private void Start()
    {
        TextWritter.AddWriter_Static(messageText, messageText.text, 0.1f, true);
        enabled = true;
    }

    private void OnEnable()
    {
        if (!enabled)
            return;

        TextWritter.AddWriter_Static(messageText, messageText.text, 0.1f, true);
    }
}
