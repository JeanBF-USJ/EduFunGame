using System;
using System.Collections.Generic;
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
    [SerializeField] private RuntimeAnimatorController playerController;
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private GameObject _player;
    private Animator _animator;
    private APIManager _apiManager;
    private LockerManager _lockerManager;
    private ShopManager _shopManager;
    private LevelManager _levelManager;
    
    private readonly Dictionary<string, float> _characterPositions = new Dictionary<string, float>()
    {
        {"Ninja", 0.0f},
        {"Archer", 0.1f},
        {"Eve", -0.15f},
        {"Kachujin", 0.165f},
        {"Vanguard", 0.12f},
    };
    

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _lockerManager = GetComponent<LockerManager>();
        _shopManager = GetComponent<ShopManager>();
        _levelManager = GetComponent<LevelManager>();
        
        SetPlayerCharacter(null);
        SetUserProfile();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void SetUserProfile()
    {
        string apiEndpoint = "/userprofile/get";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, true, HandleResponse));
    }

    private void HandleResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            UserProfileResponse response = JsonUtility.FromJson<UserProfileResponse>(www.downloadHandler.text);
            // Debug.Log("Email: " + response.email);
            // Debug.Log("Username: " + response.username);
            // Debug.Log("Birthdate: " + response.birthdate);
            
            SetPlayerCoins(response.coins);
            SetPlayerLevelProgressBarAndTextDetails(response.score);
            _lockerManager.DisplayLockerItems(response.accessories);
        }
    }
    
    public int GetPlayerCoins()
    {
        return int.Parse(coinsText.text);
    }

    public void SetPlayerCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }


    private void SetPlayerLevelProgressBarAndTextDetails(int score)
    {
        int scalingFactor = 100;
        _levelManager.SetPlayerLevel(score, scalingFactor);
    }

    public void SetPlayerCharacter(string playerCharacter)
    {
        if (playerParent.transform.childCount > 2) {
            Destroy(playerParent.transform.GetChild(2).gameObject);
            _animator = null;
            _player = null;
        }
        
        if (string.IsNullOrEmpty(playerCharacter)) playerCharacter = PlayerPrefs.GetString("playerCharacter");
        if (string.IsNullOrEmpty(playerCharacter)) playerCharacter = "Ninja";
        
        _player = Instantiate((UnityEngine.Object)Resources.Load("PlayerPrefabs/" + playerCharacter), Vector3.zero, Quaternion.identity, playerParent.transform) as GameObject;
        _player.transform.localPosition = new Vector3(0.0f, _characterPositions.ContainsKey(playerCharacter) ? _characterPositions[playerCharacter] : 0.0f, 0.0f);
        _player.transform.localRotation = Quaternion.identity;
        
        _animator = _player.AddComponent<Animator>();
        _animator.runtimeAnimatorController = playerController;
        // _animator.SetBool("isJogging", true);
    }

    public void GoToLobby()
    {
        SetPlayerCharacter(null);
        playerParent.GetComponent<Animator>().SetBool("MoveRight", false);
        if (!lobbyScreen.activeInHierarchy) lobbyScreen.SetActive(true);
        if (lockerScreen.activeInHierarchy) lockerScreen.SetActive(false);
        if (shopScreen.activeInHierarchy) shopScreen.SetActive(false);
    }

    public void GoToLocker()
    {
        SetPlayerCharacter(null);
        _lockerManager.ResetPlayerInfo();
        playerParent.GetComponent<Animator>().SetBool("MoveRight", true);
        if (!lockerScreen.activeInHierarchy) lockerScreen.SetActive(true);
        if (lobbyScreen.activeInHierarchy) lobbyScreen.SetActive(false);
        if (shopScreen.activeInHierarchy) shopScreen.SetActive(false);
    }

    public void GoToShop()
    {
        _shopManager.ResetPlayerInfo();
        playerParent.GetComponent<Animator>().SetBool("MoveRight", true);
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
    public UserAccessory[] accessories;
}

[Serializable]
public class UserAccessory
{
    public string _id;
    public string name;
    public string description;
}