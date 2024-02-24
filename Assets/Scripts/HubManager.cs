using System;
using TMPro;
using UnityEngine;
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
        if (!string.IsNullOrEmpty(savedToken))
        {
            coinsText.text = "1000";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
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