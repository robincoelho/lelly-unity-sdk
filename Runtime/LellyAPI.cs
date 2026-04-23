using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Lelly.SDK
{
    public class LellyAPI : MonoBehaviour
    {
        private static LellyAPI _instance;
        public static LellyAPI Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("LellyAPI");
                    _instance = go.AddComponent<LellyAPI>();
                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        [Header("Configuração")]
        public string apiKey;
        public string apiBaseUrl = "https://lelly.chat/api/v1";

        public void Initialize(string key)
        {
            apiKey = key;
        }

        #region Chat Endpoints
        public void CreateSession(string botSlug, string userName, string userEmail, string systemInstruction, Action<SessionResponse> onSuccess, Action<string> onError)
        {
            var data = new SessionRequest { 
                bot_slug = botSlug, 
                user = new UserData { name = userName, email = userEmail },
                system_instruction = systemInstruction
            };
            StartCoroutine(PostRequest("/chat.php?action=session", JsonUtility.ToJson(data), onSuccess, onError));
        }

        public void SendMessage(string sessionId, string message, Action<MessageResponse> onSuccess, Action<string> onError)
        {
            var data = new MessageRequest { session_id = sessionId, message = message };
            StartCoroutine(PostRequest("/chat.php?action=message", JsonUtility.ToJson(data), onSuccess, onError));
        }
        #endregion

        #region Generate Endpoints
        public void GenerateContent(string prompt, Action<GenerateResponse> onSuccess, Action<string> onError)
        {
            var data = new GenerateRequest { prompt = prompt };
            StartCoroutine(PostRequest("/generate.php", JsonUtility.ToJson(data), onSuccess, onError));
        }
        #endregion

        #region Internal Helper
        private IEnumerator PostRequest<T>(string endpoint, string json, Action<T> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = new UnityWebRequest(apiBaseUrl + endpoint, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + apiKey);

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(request.error + ": " + request.downloadHandler.text);
                }
                else
                {
                    try
                    {
                        T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                        onSuccess?.Invoke(response);
                    }
                    catch (Exception e)
                    {
                        onError?.Invoke("JSON Parse Error: " + e.Message);
                    }
                }
            }
        }
        #endregion
    }

    #region Data Models
    [Serializable] public class SessionRequest { public string bot_slug; public UserData user; public string system_instruction; }
    [Serializable] public class UserData { public string name; public string email; }
    [Serializable] public class MessageRequest { public string session_id; public string message; }
    [Serializable] public class GenerateRequest { public string prompt; }

    [Serializable] public class SessionResponse { public string session_id; public string status; }
    [Serializable] public class MessageResponse { public string reply; public string status; }
    [Serializable] public class GenerateResponse { public string content; public string status; }
    #endregion
}
