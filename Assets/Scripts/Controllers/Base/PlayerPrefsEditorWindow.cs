using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class PlayerPrefsEditorWindow : EditorWindow
{
    [MenuItem("Tools/Clear Data")]
    static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif