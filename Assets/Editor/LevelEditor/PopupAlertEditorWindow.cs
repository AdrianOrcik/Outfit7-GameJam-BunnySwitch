using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEditor;

public class PopupAlertEditorWindow : EditorWindow
{
    private static string textMessage = "";

    public static void ShowPopupWindow(string _text)
    {
        EditorWindow window = GetWindow<PopupAlertEditorWindow>("Alert!", true);
        window.maxSize = new Vector2(500f, 150f);
        window.minSize = window.maxSize;
        textMessage = _text;
    }

    private void OnGUI()
    {
        GUILayout.Label(textMessage);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Okay"))
        {
            Close();
        }

        GUILayout.FlexibleSpace();
    }
}