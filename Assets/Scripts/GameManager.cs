using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _score;
    private static GameManager _instance;
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Awake()
    {
        _instance = this;
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public void IncrementScore()
    {
        scoreText.text = "SCORE: " + ++_score;
    }
}
