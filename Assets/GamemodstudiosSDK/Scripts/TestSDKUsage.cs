using UnityEngine;

public class TestSDKUsage : MonoBehaviour
{
    private UserManager userManager;
    private GamemodstudiosAPI api;
    private AnalyticsManager analyticsManager;
    private ModManager modManager;

    private void Start()
    {
        userManager = gameObject.AddComponent<UserManager>();
        api = gameObject.AddComponent<GamemodstudiosAPI>();
        analyticsManager = gameObject.AddComponent<AnalyticsManager>();
        modManager = gameObject.AddComponent<ModManager>();

        // Test User Management
        userManager.SignUp("testUser", "password123");
        userManager.Login("testUser", "password123");

        // Test API Call
        StartCoroutine(api.GetGameData("exampleGameId", OnGameDataSuccess, OnGameDataError));

        // Test Analytics
        analyticsManager.TrackEvent("Test Event", "Event Data Example");

        // Test Mod Authentication
        modManager.AuthenticateMod("exampleModId");
    }

    private void OnGameDataSuccess(string data)
    {
        Debug.Log("Game data retrieved: " + data);
    }

    private void OnGameDataError(string error)
    {
        Debug.LogError("Error retrieving game data: " + error);
    }
}
