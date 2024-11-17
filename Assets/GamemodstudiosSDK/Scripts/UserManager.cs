using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour
{
    private const string BaseUrl = "https://api.gamemodstudios.com/";

    public void SignUp(string username, string password)
    {
        // Call the API to register a new user
        StartCoroutine(SignUpCoroutine(username, password));
    }

    private IEnumerator SignUpCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(BaseUrl + "register", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error during sign up: " + www.error);
            }
            else
            {
                Debug.Log("User signed up successfully!");
            }
        }
    }

    public void Login(string username, string password)
    {
        // Call the API to log in a user
        StartCoroutine(LoginCoroutine(username, password));
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(BaseUrl + "login", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error during login: " + www.error);
            }
            else
            {
                Debug.Log("User logged in successfully!");
            }
        }
    }

    public void Logout()
    {
        // Handle user logout
        Debug.Log("User logged out.");
    }

    // Placeholder for user profile retrieval
    public void GetUserProfile(string userId)
    {
        // Retrieve user profile from the API
        Debug.Log($"Retrieving profile for user {userId}.");
    }
}
