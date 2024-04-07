using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerParent;
    public RuntimeAnimatorController playerController;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI coinsText;

    private bool _won;
    private int _coins;
    private Animator _animator;
    private APIManager _apiManager;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        
        string playerCharacter = PlayerPrefs.GetString("playerCharacter");
        if (string.IsNullOrEmpty(playerCharacter)) playerCharacter = "Ninja";
        
        Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
        GameObject player = Instantiate((UnityEngine.Object)Resources.Load("PlayerPrefabs/" + playerCharacter), spawnPosition, Quaternion.identity, playerParent.transform) as GameObject;
        
        _animator = player.AddComponent<Animator>();
        _animator.runtimeAnimatorController = playerController;
        _animator.SetBool("isJogging", true);
    }

    public void IncrementCoins()
    {
        _coins++;
        coinsText.text = _coins.ToString();
    }

    public void GameOver(bool won)
    {
        _won = won;
        _animator.SetBool("isJogging", false);
        _animator.SetBool("isDead", true);
        SaveProgress();
        Invoke(nameof(DisplayGameOverScreen), 2);
    }

    private void SaveProgress()
    {
        string apiEndpoint = "/userprofile/update";
        string gameName = "TriviaGame";
        string jsonStr = "{\"name\":\"" + gameName + "\",\"coins\":\"" + _coins + "\",\"score\":\"" + GetScore() +
                         "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, response => { }));
    }

    private void DisplayGameOverScreen()
    {
        int indexOfPlayerScoreTextInsideGameOverUIPanel = 0;
        int indexOfCoinsCollectedTextInsideGameOverUIPanel = 1;
        int indexOfGameOverDescriptionInsideGameOverUIPanel = 2;

        TextMeshProUGUI scoreTextOnGameOverUI = gameOverUI.transform
            .GetChild(indexOfPlayerScoreTextInsideGameOverUIPanel)
            .GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI coinsTextOnGameOverUI = gameOverUI.transform
            .GetChild(indexOfCoinsCollectedTextInsideGameOverUIPanel)
            .GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI gameOverDescription = gameOverUI.transform
            .GetChild(indexOfGameOverDescriptionInsideGameOverUIPanel)
            .GetComponent<TextMeshProUGUI>();

        gameOverUI.SetActive(true);
        
        scoreTextOnGameOverUI.text = "SCORE: " + GetScore();
        coinsTextOnGameOverUI.text = "Coins Collected: " + _coins;
        gameOverDescription.text = _won ? "You completed the game!" : "You chose the wrong answer!";
    }

    private string GetScore()
    {
        return playerParent.transform.position.z.ToString("0");
    }
}