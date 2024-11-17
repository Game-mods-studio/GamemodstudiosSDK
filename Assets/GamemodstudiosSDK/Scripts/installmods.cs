using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Gamemodstudios;
using System.Runtime.CompilerServices;

public class InstallMods : MonoBehaviour
{
    [SerializeField] private string modListUrl = Gamemodstudiosinit.BaseUrl; // Ensure this URL is correct

    [SerializeField] private TMP_Text modListText; // TextMeshPro object to display mod list

    private void Start()
    {
        
        StartCoroutine(DownloadModList());
    }

private IEnumerator DownloadModList()
{
    Debug.Log("Requesting mod list from: " + modListUrl); // Log the URL being requested

    using (UnityWebRequest webRequest = UnityWebRequest.Get(modListUrl))
    {
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading mod list: " + webRequest.error);
        }
        else
        {
            // Process the response
            string json = webRequest.downloadHandler.text;
            Debug.Log("Received JSON: " + json); // Log the received JSON
            Mod[] mods = JsonHelper.FromJson<Mod>(json);
            DisplayModList(new List<Mod>(mods));
        }
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        // Fix the JSON format
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}


    private void DisplayModList(List<Mod> mods)
    {
        modListText.text = ""; // Clear existing text

        foreach (var mod in mods)
        {
            modListText.text += $"ID: {mod.id}, Title: {mod.title}, Version: {mod.version}, Author: {mod.author}\n";
        }
    }
        
    // Public method to download a mod by ID
    public void DownloadMod(int modId, string installPath)
    {
        StartCoroutine(DownloadModCoroutine(modId));
    }

    private IEnumerator DownloadModCoroutine(int modId)
    {
    
    string downloadUrl = Gamemodstudiosinit.BaseUrl + "/" + modId;
        Debug.Log($"Downloading mod {modId} from: {downloadUrl}"); // Log the download URL
        string installPath = null;
        string downloadPath = Path.Combine(installPath, $"mod_{modId}.zip"); // Combine the install path with the mod ID
        Debug.Log("DownloadPath: " + downloadPath);

        using (UnityWebRequest request = UnityWebRequest.Get(downloadUrl))
        {
            // Download the mod as a binary file
            request.downloadHandler = new DownloadHandlerFile(downloadPath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading mod: " + request.error);
            }
            else
            {
                Debug.Log($"Mod {modId} downloaded successfully to {downloadPath}");
            }
        }
    }

[System.Serializable]
public class Mod
{
    public int id;
    public string title;
    public string version;
    public string author;
}


    [System.Serializable]
    public class ModList
    {
        public List<Mod> mods;
    }
}
