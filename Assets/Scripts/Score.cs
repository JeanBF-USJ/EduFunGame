using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI scoreText;
    void Update()
    {
        scoreText.text = player.position.z.ToString("0");
    }
}