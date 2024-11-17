using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class GamemodstudiosSDKWindow : EditorWindow
{
    private string apiKey = "";
    private bool enableAnalytics = true;
    private string modID = "defaultModID";
    private string gameID = "defaultGameID";
    private bool isModIDOptional = true;

    private const string idPattern = @"^(?<type>\d{2})(?<category>\d{3})(?<userId>\d{4})(?<date>\d{8})(?<randomId>\d{4})$";

    [MenuItem("Gamemodstudios/SDK Settings")]
    public static void ShowWindow()
    {
        GetWindow<GamemodstudiosSDKWindow>("Gamemodstudios SDK");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gamemodstudios SDK Configuration", EditorStyles.boldLabel);

        GUILayout.Label("API Key", EditorStyles.label);
        apiKey = EditorGUILayout.TextField(apiKey);

        enableAnalytics = EditorGUILayout.Toggle("Enable Analytics", enableAnalytics);

        GUILayout.Label("Game ID", EditorStyles.label);
        gameID = EditorGUILayout.TextField(gameID);
        if (!ValidateID(gameID))
        {
            EditorGUILayout.HelpBox("Invalid Game ID format. Expected format: TTCCCYYYYMMDDRRRR", MessageType.Error);
        }

        isModIDOptional = EditorGUILayout.Toggle("Make Mod ID Optional", isModIDOptional);

        GUILayout.Label("Mod ID", EditorStyles.label);
        modID = EditorGUILayout.TextField(modID);
        if (!isModIDOptional && !ValidateID(modID))
        {
            EditorGUILayout.HelpBox("Invalid Mod ID format. Expected format: TTCCCYYYYMMDDRRRR", MessageType.Error);
        }

        if (GUILayout.Button("Authenticate User"))
        {
            AuthenticateUser();
        }

        if (GUILayout.Button("Fetch Game Data"))
        {
            FetchGameData();
        }

        if (GUILayout.Button("Save Settings"))
        {
            SaveSettings();
        }

        // Reset button to restore default values
        if (GUILayout.Button("Reset to Default Values"))
        {
            ResetToDefaults();
        }
    }

    private void AuthenticateUser()
    {
        Debug.Log("Authenticating user with API Key: " + apiKey);
    }

    private void FetchGameData()
    {
        Debug.Log("Fetching game data for Game ID: " + gameID + " and Mod ID: " + modID);
    }

    private void SaveSettings()
    {
        EditorPrefs.SetString("GamemodstudiosAPIKey", apiKey);
        EditorPrefs.SetBool("GamemodstudiosAnalyticsEnabled", enableAnalytics);
        EditorPrefs.SetString("GamemodstudiosModID", modID);
        EditorPrefs.SetString("GamemodstudiosGameID", gameID);

        Debug.Log("Settings saved!");
        EditorGUILayout.HelpBox("Settings saved!", MessageType.Info);
    }

    private void ResetToDefaults()
    {
        apiKey = "";
        enableAnalytics = false;
        modID = "defaultModID";
        gameID = "defaultGameID";
        isModIDOptional = false;

        Debug.Log("Reset to default values.");
    }

    private static bool ValidateID(string id)
    {
        Match match = Regex.Match(id, idPattern);
        if (!match.Success)
        {
            return false;
        }

        string dateStr = match.Groups["date"].Value;
        return IsValidDate(dateStr);
    }

    private static bool IsValidDate(string dateStr)
    {
        if (dateStr.Length != 8) return false;

        int year = int.Parse(dateStr.Substring(0, 4));
        int month = int.Parse(dateStr.Substring(4, 2));
        int day = int.Parse(dateStr.Substring(6, 2));

        try
        {
            DateTime date = new DateTime(year, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
