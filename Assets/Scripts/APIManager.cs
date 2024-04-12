using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    private string _baseUrl = "http://localhost:3000";

    public delegate void ResponseCallback(UnityWebRequest response);
    
    public IEnumerator SendRequest(string apiEndpoint, string jsonStr, bool requiredToken, ResponseCallback callback)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(_baseUrl + apiEndpoint, "POST"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            if (requiredToken)
            {
                string savedToken = PlayerPrefs.GetString("token");
                if (string.IsNullOrEmpty(savedToken))
                {
                    Logout();
                    yield break;
                }
                else www.SetRequestHeader("Authorization", savedToken);
            }

            if (jsonStr == null) jsonStr = "{}";
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);

            yield return www.SendWebRequest();

            if (requiredToken && www.responseCode == 401) Logout();
            else callback(www);
        }
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
