using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


    public class GamemodstudiosSupabaseClient : MonoBehaviour
    {
        private const string SupabaseUrl = "https://localhost:8000";
        private const string SupabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJnYmpwY3V2ZnBzeXhlaWpoaGthIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mjg4MzM4NDUsImV4cCI6MjA0NDQwOTg0NX0.1vpncyA4eXuUnlrxLM9r0736GQ76Yrk7Oy_BKNqXvqA";
        private string authToken = string.Empty;

        // Method to set the authentication token
        public void SetAuthToken(string token)
        {
            authToken = token;
        }

        // Method to authenticate the user using Supabase
        public async Task<string> AuthenticateUser(string email, string password)
        {
            string url = $"{SupabaseUrl}/auth/v1/token?grant_type=password";
            Dictionary<string, string> formData = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password }
            };

            using (UnityWebRequest request = UnityWebRequest.Post(url, formData))
            {
                request.SetRequestHeader("apikey", SupabaseApiKey);
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string response = request.downloadHandler.text;
                    // Extract and set the auth token here
                    // e.g., authToken = ExtractTokenFromResponse(response);
                    return response;
                }
                else
                {
                    Debug.LogError($"Authentication error: {request.error}");
                    return null;
                }
            }
        }

        // Example method to make a GET request to fetch game information using Supabase
        public async Task<string> GetGameInfo(string gameId)
        {
            string url = $"{SupabaseUrl}/rest/v1/games?id=eq.{gameId}";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("apikey", SupabaseApiKey);
                if (!string.IsNullOrEmpty(authToken))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {authToken}");
                }

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError($"Error fetching game info: {request.error}");
                    return null;
                }
            }
        }
    }

