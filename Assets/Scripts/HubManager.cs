using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject lobbyScreen;
    [SerializeField] private GameObject lockerScreen;
    [SerializeField] private GameObject shopScreen;
    
    [Header("PlayerInfo")]
    [SerializeField] private GameObject playerParent;
    [SerializeField] private TextMeshProUGUI coinsText;
    private APIManager _apiManager;
    
    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        
        string savedToken = PlayerPrefs.GetString("token");
        if (string.IsNullOrEmpty(savedToken)) _apiManager.Logout();
        else
        {
            string apiEndpoint = "/user/profile";
            StartCoroutine(_apiManager.SendRequest(apiEndpoint, savedToken, null, HandleResponse));
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    private void HandleResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            UserProfileResponse response = JsonUtility.FromJson<UserProfileResponse>(www.downloadHandler.text);
            Debug.Log("Email: " + response.email);
            Debug.Log("Username: " + response.username);
            Debug.Log("Birthdate: " + response.birthdate);
            Debug.Log("Coins: " + response.coins);
            foreach (string accessory in response.accessories)
            {
                Debug.Log("Accessory: " + accessory);
            }

            coinsText.text = response.coins.ToString();
        }
    }

    public void GoToLobby()
    {
        Debug.Log(lobbyScreen.activeInHierarchy);
        Debug.Log(lobbyScreen.activeSelf);
        if (!lobbyScreen.activeInHierarchy) lobbyScreen.SetActive(true);
        if (lockerScreen.activeInHierarchy) lockerScreen.SetActive(false);
        if (shopScreen.activeInHierarchy) shopScreen.SetActive(false);
    }

    public void GoToLocker()
    {
        if (!lockerScreen.activeInHierarchy) lockerScreen.SetActive(true);
        if (lobbyScreen.activeInHierarchy) lobbyScreen.SetActive(false);
        if (shopScreen.activeInHierarchy) shopScreen.SetActive(false);
    }

    public void GoToShop()
    {
        if (!shopScreen.activeInHierarchy) shopScreen.SetActive(true);
        if (lobbyScreen.activeInHierarchy) lobbyScreen.SetActive(false);
        if (lockerScreen.activeInHierarchy) lockerScreen.SetActive(false);
    }
}

[Serializable]
public class UserProfileResponse
{
    public string email;
    public string username;
    public string birthdate;
    public int coins;
    public string[] accessories;
}