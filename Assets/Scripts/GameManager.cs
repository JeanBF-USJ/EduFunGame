using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerParent;
    [SerializeField] private GameObject playerPrefab;
    public RuntimeAnimatorController playerController;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private int _coins;
    private Animator _animator;
    private APIManager _apiManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        
        Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, playerParent.transform);
        _animator = player.AddComponent<Animator>();
        _animator.runtimeAnimatorController = playerController;
        _animator.SetBool("isJogging", true);
    }

    public void IncrementCoins()
    {
        _coins++;
        coinsText.text =  _coins.ToString();
    }

    public void GameOver()
    {
        _animator.SetBool("isJogging",false);
        _animator.SetBool("isDead",true);
        SaveProgress();
        Invoke(nameof(DisplayGameOverScreen), 2);
    }

    private void SaveProgress()
    {
        string savedToken = PlayerPrefs.GetString("token");
        if (string.IsNullOrEmpty(savedToken)) _apiManager.Logout();
        else
        {
            string apiEndpoint = "/userprofile/update";
            string gameID = "65df8a027bcad920cb100aac";
            string jsonStr = "{\"game_id\":\"" + gameID + "\",\"coins\":\"" + _coins + "\",\"score\":\"" + GetScore() + "\"}";
            StartCoroutine(_apiManager.SendRequest(apiEndpoint, savedToken, jsonStr, response => {}));
        }
    }

    private void DisplayGameOverScreen()
    {
        int indexOfPlayerScoreTextInsideGameOverUIPanel = 0;
        int indexOfCoinsCollectedTextInsideGameOverUIPanel = 1;
        
        TextMeshProUGUI scoreTextOnGameOverUI = gameOverUI.transform.GetChild(indexOfPlayerScoreTextInsideGameOverUIPanel).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI coinsTextOnGameOverUI = gameOverUI.transform.GetChild(indexOfCoinsCollectedTextInsideGameOverUIPanel).GetComponent<TextMeshProUGUI>();
        
        gameOverUI.SetActive(true);
        scoreTextOnGameOverUI.text = "SCORE: " + GetScore();
        coinsTextOnGameOverUI.text = "Coins Collected: " + _coins;
    }

    private string GetScore()
    {
        return playerParent.transform.position.z.ToString("0");
    }
}