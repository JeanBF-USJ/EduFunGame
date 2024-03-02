using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    [Header("Screens")] [SerializeField] private GameObject lobbyScreen;
    [SerializeField] private GameObject lockerScreen;
    [SerializeField] private GameObject shopScreen;

    [Header("PlayerInfo")] [SerializeField]
    private GameObject playerParent;

    [SerializeField] private TextMeshProUGUI coinsText;

    private APIManager _apiManager;
    private LevelCalculator _levelCalculator;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _levelCalculator = GetComponent<LevelCalculator>();

        string apiEndpoint = "/userprofile/get";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, true, HandleResponse));
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

            SetPlayerCoins(response.coins);
            SetPlayerLevelProgressBarAndTextDetails(response.score);
        }
    }

    private void SetPlayerCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }

    private void SetPlayerLevelProgressBarAndTextDetails(int score)
    {
        int scalingFactor = 100;
        LevelInfo levelInfo = _levelCalculator.CalculateLevelAndProgress(score, scalingFactor);

        // SET UI BAR & TEXT
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
    public int score;
    public string[] accessories;
}