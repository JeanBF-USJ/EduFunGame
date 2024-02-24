using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    private string _baseUrl = "http://localhost:3000";

    public delegate void AuthCallback(UnityWebRequest response);
    
    public IEnumerator SendRequest(string apiEndpoint, string jsonStr, AuthCallback callback)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(_baseUrl + apiEndpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            callback(www);
        }
    }
}
