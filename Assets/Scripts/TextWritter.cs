using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWritter : MonoBehaviour
{
    List<TextWriterSingle> textWriterSingle;

    public static TextWritter instance;
    private void Awake()
    {
        instance = this;
        textWriterSingle = new List<TextWriterSingle>();
    }

    public static void AddWriter_Static(Text _textUI, string _text_to_write, float _time_per_character, bool _invisible_character)
    {
        instance.AddWriter(_textUI, _text_to_write, _time_per_character, _invisible_character);
    }
    void AddWriter(Text _textUI, string _text_to_write, float _time_per_character, bool _invisible_character)
    {
        _text_to_write = _text_to_write.Replace("<color=#00000000>", "");
        _text_to_write = _text_to_write.Replace("</color>", "");

        textWriterSingle.Add(new TextWriterSingle(_textUI, _text_to_write, _time_per_character, _invisible_character));
    }

    private void Update()
    {
        for (int i = 0; i < textWriterSingle.Count; i++)
        {
            bool destroyInstance = textWriterSingle[i].Update();
            if (destroyInstance)
            {
                textWriterSingle.RemoveAt(i);
                i--;
            }
        }

    }

}

public class TextWriterSingle
{
    Text textUI;
    string textToWrite;
    int characterIndex;
    float timePerCharacter;
    float timer;
    bool invisibleCharacter;

    public TextWriterSingle(Text _textUI, string _text_to_write, float _time_per_character, bool _invisible_character)
    {
        textUI = _textUI;
        textToWrite = _text_to_write;
        timePerCharacter = _time_per_character;
        invisibleCharacter = _invisible_character;
        characterIndex = 0;
    }
    public bool Update()
    {
        timer -= Time.deltaTime;
        while (timer <= 0f)
        {
            timer += timePerCharacter;
            characterIndex++;
            string text = textToWrite.Substring(0, characterIndex);

            if (invisibleCharacter)
            {
                text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
            }
            textUI.text = text;

            if (characterIndex >= textToWrite.Length)
            {
                return true;
            }
        }
        return false;
    }
}
