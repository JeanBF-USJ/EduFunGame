using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    [SerializeField] private GameObject serverOfflineText;
    
    private string _protocol = "http://";
    private string _port = ":1337";
    private string _baseUrl = "/api";
    private string _ip;

    private void Awake()
    {
#if UNITY_EDITOR
        PlayerSettings.insecureHttpOption = InsecureHttpOption.AlwaysAllowed;
#endif
        string filePath = Path.Combine(Application.streamingAssetsPath, "config.json");
        string dataAsJson = File.ReadAllText(filePath);
        ConfigData configData = JsonUtility.FromJson<ConfigData>(dataAsJson);
        _ip = configData.IP;
    }
    
    public delegate void ResponseCallback(UnityWebRequest response);
    
    public IEnumerator SendRequest(string apiEndpoint, string jsonStr, bool requiredToken, ResponseCallback callback)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(_protocol + _ip + _port + _baseUrl + apiEndpoint, "POST"))
        {
            www.timeout = 15;
            if (SceneManager.GetActiveScene().buildIndex == 0 && serverOfflineText.activeSelf) serverOfflineText.SetActive(false);
            
            www.SetRequestHeader("Content-Type", "application/json");
            if (requiredToken)
            {
                string savedToken = PlayerPrefs.GetString("token");
                if (string.IsNullOrEmpty(savedToken))
                {
                    Logout();
                    yield break;
                }
                
                www.SetRequestHeader("Authorization", savedToken);
            }

            if (jsonStr == null) jsonStr = "{}";
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);

            yield return www.SendWebRequest();
            
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                if (SceneManager.GetActiveScene().buildIndex != 0) Logout();
                if (SceneManager.GetActiveScene().buildIndex == 0 && !serverOfflineText.activeSelf) serverOfflineText.SetActive(true);
            }
            else if (requiredToken && www.responseCode == 401) Logout();
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

[Serializable]
public class ConfigData
{
    public string IP;
}