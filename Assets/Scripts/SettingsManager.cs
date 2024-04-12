using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private Image accountButton;
    [SerializeField] private GameObject accountScreen;
    [SerializeField] private Image creditsButton;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private Image legalButton;
    [SerializeField] private GameObject legalScreen;
    
    [Header("Settings")]
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TextMeshProUGUI oldPasswordTitle;
    [SerializeField] private TMP_InputField oldPasswordField;
    [SerializeField] private TMP_InputField newPasswordField;
    [SerializeField] private GameObject newPasswordSection;
    
    [SerializeField] private GameObject editUsernameButton;
    [SerializeField] private GameObject cancelEditUsernameButton;
    [SerializeField] private GameObject confirmEditUsernameButton;
    
    [SerializeField] private GameObject editPasswordButton;
    [SerializeField] private GameObject cancelEditPasswordButton;
    
    [SerializeField] private TextMeshProUGUI usernameErrorText;
    [SerializeField] private TextMeshProUGUI passwordErrorText;
    
    private string _username;
    private APIManager _apiManager;
    private HubManager _hubManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _hubManager = GetComponent<HubManager>();
    }

    public void GoToAccount()
    {
        if (!accountScreen.activeInHierarchy)
        {
            accountScreen.SetActive(true);
            accountButton.color = new Color(1f, 1f, 1f, 1f);
        }
        if (creditsScreen.activeInHierarchy)
        {
            creditsScreen.SetActive(false);
            creditsButton.color = new Color(1f, 1f, 1f, 0);
        }

        if (legalScreen.activeInHierarchy)
        {
            legalScreen.SetActive(false);
            legalButton.color = new Color(1f, 1f, 1f, 0);
        }
    }
    
    public void GoToCredits()
    {
        if (!creditsScreen.activeInHierarchy)
        {
            creditsScreen.SetActive(true);
            creditsButton.color = new Color(1f, 1f, 1f, 1f);
        }
        if (accountScreen.activeInHierarchy)
        {
            accountScreen.SetActive(false);
            accountButton.color = new Color(1f, 1f, 1f, 0);
        }
        if (legalScreen.activeInHierarchy)
        {
            legalScreen.SetActive(false);
            legalButton.color = new Color(1f, 1f, 1f, 0);
        }
    }
    
    public void GoToLegal()
    {
        if (!legalScreen.activeInHierarchy)
        {
            legalScreen.SetActive(true);
            legalButton.color = new Color(1f, 1f, 1f, 1f);
        }
        if (accountScreen.activeInHierarchy)
        {
            accountScreen.SetActive(false);
            accountButton.color = new Color(1f, 1f, 1f, 0);
        }
        if (creditsScreen.activeInHierarchy)
        {
            creditsScreen.SetActive(false);
            creditsButton.color = new Color(1f, 1f, 1f, 0);
        }
    }

    public void SetEmail(string email)
    {
        emailField.text = email;
    }

    public void SetUsername(string username)
    {
        _username = username;
        usernameField.text = username;
    }

    public void EditUsername()
    {
        ResetErrorMessages();
        editUsernameButton.SetActive(false);
        cancelEditUsernameButton.SetActive(true);
        confirmEditUsernameButton.SetActive(true);
        usernameField.readOnly = false;
        usernameField.Select();
    }

    public void CancelEditUsername()
    {
        HandlePostEditUsernameActions();
        usernameField.text = _username;
    }

    public void ConfirmEditUsername()
    {
        HandlePostEditUsernameActions();

        string newUsername = usernameField.text;
        if (_username == newUsername) return;

        string apiEndpoint = "/auth/update/username";
        string jsonStr = "{\"username\":\"" + newUsername + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, EditUsernameResponse));
    }

    private void HandlePostEditUsernameActions()
    {
        editUsernameButton.SetActive(true);
        cancelEditUsernameButton.SetActive(false);
        confirmEditUsernameButton.SetActive(false);
        usernameField.readOnly = true;
    }
    
    public void EditUsernameResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            EditAccountSuccess response = JsonUtility.FromJson<EditAccountSuccess>(www.downloadHandler.text);
            
            PlayerPrefs.SetString("token", response.token);
            PlayerPrefs.Save();
            
            SetUsername(response.username);
            _hubManager.SetUsername(response.username);
        }
        else if (www.responseCode == 400) {
            EditAccountError response = JsonUtility.FromJson<EditAccountError>(www.downloadHandler.text);
            usernameField.text = _username;
            usernameErrorText.text = response.error;
        }
        else usernameErrorText.text = "Something went wrong!";
    }
    
    public void EditPassword()
    {
        ResetErrorMessages();
        editPasswordButton.SetActive(false);
        cancelEditPasswordButton.SetActive(true);
        newPasswordSection.SetActive(true);
        oldPasswordTitle.text = "Old Password";
        oldPasswordField.readOnly = false;
        oldPasswordField.text = "";
        oldPasswordField.Select();
    }

    public void CancelEditPassword()
    {
        HandlePostEditPasswordActions();
    }

    public void ConfirmEditPassword()
    {
        string oldPassword = oldPasswordField.text;
        string newPassword = newPasswordField.text;
        
        HandlePostEditPasswordActions();
        
        if (oldPassword == newPassword) passwordErrorText.text = "You should have a different password in both fields";
        else
        {
            string apiEndpoint = "/auth/update/password";
            string jsonStr = "{\"email\":\"" + emailField.text + "\",\"oldPassword\":\"" + oldPassword + "\",\"newPassword\":\"" + newPassword + "\"}";
            StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, EditPasswordResponse)); 
        }
    }

    private void HandlePostEditPasswordActions()
    {
        editPasswordButton.SetActive(true);
        cancelEditPasswordButton.SetActive(false);
        newPasswordSection.SetActive(false);
        oldPasswordTitle.text = "Password";
        oldPasswordField.readOnly = true;
        oldPasswordField.text = "Enter your old password...";
        newPasswordField.text = "";
    }

    public void EditPasswordResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            EditAccountSuccess response = JsonUtility.FromJson<EditAccountSuccess>(www.downloadHandler.text);
            
            PlayerPrefs.SetString("token", response.token);
            PlayerPrefs.Save();
        }
        else if (www.responseCode == 400) {
            EditAccountError response = JsonUtility.FromJson<EditAccountError>(www.downloadHandler.text);
            passwordErrorText.text = response.error;
        }
        else passwordErrorText.text = "Something went wrong!";
    }

    public void ResetErrorMessages()
    {
        usernameErrorText.text = "";
        passwordErrorText.text = "";
    }

    public void Logout()
    {
        _apiManager.Logout();
    }
}

[Serializable]
public class EditAccountSuccess
{
    public string username;
    public string token;
}

[Serializable]
public class EditAccountError
{
    public string error;
}