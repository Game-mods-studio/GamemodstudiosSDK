using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Gamemodstudios;

public class GamemodstudiosAPI : MonoBehaviour
{
    private const string BaseUrl = Gamemodstudiosinit.BaseUrl;

    public IEnumerator GetGameData(string gameId, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(BaseUrl + "games/" + gameId))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(www.error);
            }
            else
            {
                onSuccess?.Invoke(www.downloadHandler.text);
            }
        }
    }
}
