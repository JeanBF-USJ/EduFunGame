using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject registerScreen;

    [Header("LoginFields")]
    [SerializeField] private TMP_InputField loginEmailField;
    [SerializeField] private TMP_InputField loginPasswordField;
    [SerializeField] private TextMeshProUGUI loginErrorText;
    
    [Header("RegisterFields")]
    [SerializeField] private TMP_InputField registerUsernameField;
    [SerializeField] private TMP_InputField registerEmailField;
    [SerializeField] private TMP_InputField registerPasswordField;
    [SerializeField] private TMP_InputField registerBirthdateField;
    [SerializeField] private TextMeshProUGUI registerErrorText;
    
    private APIManager _apiManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        string savedToken = PlayerPrefs.GetString("token");
        if (!string.IsNullOrEmpty(savedToken))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void AlreadyHaveAnAccount()
    {
        loginScreen.gameObject.SetActive(true);
        registerScreen.gameObject.SetActive(false);
    }
    
    public void DontHaveAnAccount()
    {
        registerScreen.gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
    }

    public void Login()
    {
        string apiEndpoint = "/auth/login";
        string jsonStr = "{\"email\":\"" + loginEmailField.text + "\",\"password\":\"" + loginPasswordField.text + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, jsonStr, HandleResponse));
    }

    public void Register()
    {
        string apiEndpoint = "/auth/register";
        string jsonStr = "{\"username\":\"" + registerUsernameField.text + "\",\"email\":\"" + registerEmailField.text
                         + "\",\"password\":\"" + registerPasswordField.text + "\",\"birthdate\":\"" + registerBirthdateField.text + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, jsonStr, HandleResponse));
    }
    
    private void HandleResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            AuthResponse response = JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);
            string token = response.token;

            PlayerPrefs.SetString("token", token);
            PlayerPrefs.Save();
            SceneManager.LoadScene(1);
        }
        else
        {
            if (!string.IsNullOrEmpty(www.downloadHandler.text))
            {
                try
                {
                    var errorResponse = JsonUtility.FromJson<ErrorResponse>(www.downloadHandler.text);
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.error))
                    {
                        TextMeshProUGUI errorText = www.url.EndsWith("/auth/register") ? registerErrorText : loginErrorText;
                        errorText.text = errorResponse.error;
                    }
                    else Debug.LogError("No error response found: " + www.error );
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to parse error response: " + e.Message);
                }
            }
            else Debug.LogError("Null or empty response: " + www.error);
        }
    }
}

[System.Serializable]
public class AuthResponse
{
    public string token;
}

[Serializable]
public class ErrorResponse
{
    public string error;
}