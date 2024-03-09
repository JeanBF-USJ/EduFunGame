using System;
using TMPro;
using Unity.VisualScripting;
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
    private LevelManager _levelManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
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
            Debug.Log("Email: " + response.email);
            Debug.Log("Username: " + response.username);
            Debug.Log("Birthdate: " + response.birthdate);
            Debug.Log("Coins: " + response.coins);
            foreach (string accessory in response.accessories)
            {
                Debug.Log("Accessory: " + accessory);
            }

            SetPlayerCharacter();
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
        _levelManager.SetPlayerLevel(score, scalingFactor);
    }

    private void SetPlayerCharacter()
    {
        if (playerParent.transform.childCount > 0) {
            Destroy(playerParent.transform.GetChild(0).gameObject);
            _animator = null;
            _player = null;
        }
        
        string playerCharacter = PlayerPrefs.GetString("playerCharacter");
        if (string.IsNullOrEmpty(playerCharacter)) playerCharacter = "Ninja";
        
        _player = Instantiate((UnityEngine.Object)Resources.Load(playerCharacter), Vector3.zero, Quaternion.identity, playerParent.transform) as GameObject;
        _player.transform.localPosition = Vector3.zero;
        _player.transform.localRotation = Quaternion.identity;
        
        _animator = _player.AddComponent<Animator>();
        _animator.runtimeAnimatorController = playerController;
        // _animator.SetBool("isJogging", true);
    }

    public void GoToLobby()
    {
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