using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class GamemodstudiosWindow : EditorWindow
{

    [MenuItem("Gamemodstudios/Settings")]
    public static void ShowWindow()
    {
        GetWindow<GamemodstudiosWindow>("Gamemodstudios Settings");
    }

private void OnGUI()
    {
        GUILayout.Label("Gamemodstudios Configuration", EditorStyles.boldLabel);







}
}