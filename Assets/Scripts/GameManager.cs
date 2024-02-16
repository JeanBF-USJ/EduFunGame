using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _score;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void IncrementScore()
    {
        _score++;
        scoreText.text =  _score.ToString();
    }

    public void DisplayGameOverScreen()
    {
        gameOverUI.SetActive(true);
    }
}
