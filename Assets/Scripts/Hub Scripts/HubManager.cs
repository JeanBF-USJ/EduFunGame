using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject lobbyScreen;
    [SerializeField] private GameObject lockerScreen;
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private GameObject settingsScreen;

    [Header("PlayerInfo")]
    [SerializeField] private GameObject playerParent;
    [SerializeField] private RuntimeAnimatorController playerController;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI coinsText;
    
    [Header("")]
    [SerializeField] private Button playGameButton;
    [SerializeField] private RawImage selectedGameImage;
    
    private GameObject _player;
    private Animator _animator;
    private APIManager _apiManager;
    private LockerManager _lockerManager;
    private ShopManager _shopManager;
    private LevelManager _levelManager;
    private GameSelectionManager _gameSelectionManager;
    private GameDescriptionManager _gameDescriptionManager;
    private SettingsManager _settingsManager;
    
    private string _id;
    private DateTime _birthdate;

    private string _selectedGame = "TriviaGame";
    
    private readonly Dictionary<string, float> _characterPositions = new Dictionary<string, float>()
    {
        {"Ninja", 0.0f},
        {"Archer", 0.1f},
        {"Eve", -0.15f},
        {"Kachujin", 0.165f},
        {"Vanguard", 0.12f},
    };
    
    private readonly Dictionary<string, int> _gameScenes = new Dictionary<string, int>()
    {
        {"TriviaGame", 2}
    };
    

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _lockerManager = GetComponent<LockerManager>();
        _shopManager = GetComponent<ShopManager>();
        _levelManager = GetComponent<LevelManager>();
        _gameSelectionManager = GetComponent<GameSelectionManager>();
        _gameDescriptionManager = GetComponent<GameDescriptionManager>();
        _settingsManager = GetComponent<SettingsManager>();
        
        SetUserProfile();
    }

    public void SelectGame(string gameName)
    {
        _selectedGame = gameName;
        Texture2D texture = Resources.Load<Texture2D>("GameIcons/" + gameName);
        selectedGameImage.texture = texture;

        playGameButton.interactable = _gameScenes.ContainsKey(_selectedGame);
    }

    public void StartGame()
    {
        if (_gameScenes.ContainsKey(_selectedGame)) SceneManager.LoadScene(_gameScenes[_selectedGame]);
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

            _id = response._id;
            PlayerPrefs.SetString("LoggedInUser", _id);
            PlayerPrefs.Save();
            
            _birthdate = DateTime.Parse(response.birthdate);

            _settingsManager.SetEmail(response.email);
            _settingsManager.SetUsername(response.username);
            
            SetUsername(response.username);
            SetPlayerCharacter(null);
            SetPlayerCoins(response.coins);
            SetPlayerLevelProgressBarAndTextDetails(response.score);
            _lockerManager.DisplayLockerItems(response.accessories);
            _gameSelectionManager.FillGameSelectionMenu();
            _gameDescriptionManager.FillLeaderboard("TriviaGame");
        }
    }
    
    public string GetID()
    {
        return _id;
    }
    
    public DateTime GetBirthdate()
    {
        return _birthdate;
    }
    
    public int GetPlayerCoins()
    {
        return int.Parse(coinsText.text);
    }
    
    public void SetUsername(string username)
    {
        this.username.text = username;
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
        
        if (string.IsNullOrEmpty(playerCharacter)) playerCharacter = PlayerPrefs.GetString(GetID());
        if (string.IsNullOrEmpty(playerCharacter)) playerCharacter = "Ninja";
        
        _player = Instantiate((UnityEngine.Object)Resources.Load("PlayerPrefabs/" + playerCharacter), Vector3.zero, Quaternion.identity, playerParent.transform) as GameObject;
        _player.transform.localPosition = new Vector3(0.0f, _characterPositions.ContainsKey(playerCharacter) ? _characterPositions[playerCharacter] : 0.0f, 0.0f);
        _player.transform.localRotation = Quaternion.identity;
        
        _animator = _player.AddComponent<Animator>();
        _animator.runtimeAnimatorController = playerController;
        // _animator.SetBool("isYawning", true);
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

    public void GoToSettings()
    {
        settingsScreen.SetActive(true);
    }
    
    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
        _settingsManager.ResetErrorMessages();
    }
}

[Serializable]
public class UserProfileResponse
{
    public string _id;
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