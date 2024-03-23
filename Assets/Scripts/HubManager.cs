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
    [SerializeField] private RuntimeAnimatorController playerController;
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private GameObject _player;
    private Animator _animator;
    private APIManager _apiManager;
    private LockerManager _lockerManager;
    private ShopManager _shopManager;
    private LevelManager _levelManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _lockerManager = GetComponent<LockerManager>();
        _shopManager = GetComponent<ShopManager>();
        _levelManager = GetComponent<LevelManager>();

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
            // Debug.Log("Email: " + response.email);
            // Debug.Log("Username: " + response.username);
            // Debug.Log("Birthdate: " + response.birthdate);
            
            SetPlayerCharacter(null);
            SetPlayerCoins(response.coins);
            SetPlayerLevelProgressBarAndTextDetails(response.score);
            _lockerManager.DisplayLockerItems(response.accessories);
        }
    }

    private void SetPlayerCoins(int coins)
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
        _player.transform.localPosition = Vector3.zero;
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