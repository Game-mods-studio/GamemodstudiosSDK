using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace Gamemodstudios
{
    public class Gamemodstudiosinit : MonoBehaviour
    {
        [SerializeField] private InstallMods installMods;
        public const string BaseUrl = "https://gamemodstudios-mod-server.vercel.app";
        public readonly string installPath = Path.Combine(Application.persistentDataPath, "GamemodstudiosSDK/Mods");

        void Start()
        {   
            StartCoroutine(GetRequest(BaseUrl+"/all"));
            Debug.Log("Mod folder: " + Application.persistentDataPath);
            Debug.Log("Installed mods: " + InstalledMods());
            InstallMod(0);
        }

        private string InstalledMods()
        {
            // Set the Mods folder path
            string modsFolderPath = Path.Combine(Application.persistentDataPath, "GamemodstudiosSDK/Mods");
            string installedMods = string.Empty;

            // Create the Mods folder if it doesn't exist
            if (!Directory.Exists(modsFolderPath))
            {
                Directory.CreateDirectory(modsFolderPath);
            }

            string[] modFolders = Directory.GetDirectories(modsFolderPath);
            foreach (string folder in modFolders)
            {
                string modName = Path.GetFileName(folder);
                installedMods += modName + ", ";
            }

            if (installedMods.Length > 2)
            {
                installedMods = installedMods.Substring(0, installedMods.Length - 2); // Remove trailing comma and space
            }

            return installedMods;
        }

        // Method to install a mod by ID
        public void InstallMod(int modId)
        {
            if (modId == 0)
            {
                Debug.Log("No mod selected to install.");
                return;
            }
            // Get the InstallMods component if it exists
            if (installMods != null)
            {
                installMods.DownloadMod(modId, installPath);
            }
            else
            {
                Debug.LogError("InstallMods is not assigned!");
            }
        }



public void Setup()
{
    string ConfigFolderPath = Path.Combine(Application.persistentDataPath, "GamemodstudiosSDK/Config");

    if (!Directory.Exists(ConfigFolderPath))
    {
        Directory.CreateDirectory(ConfigFolderPath);
    }
}


IEnumerator GetRequest(string uri)
{
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
}




}}
