using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _score;
    private Animator _animator;
    [SerializeField] private GameObject playerParent;
    [SerializeField] private GameObject playerPrefab;
    public RuntimeAnimatorController playerController;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, playerParent.transform);
        _animator = player.AddComponent<Animator>();
        _animator.runtimeAnimatorController = playerController;
        _animator.SetBool("isJogging", true);
    }

    public void IncrementScore()
    {
        _score++;
        scoreText.text =  _score.ToString();
    }

    public void GameOver()
    {
        _animator.SetBool("isJogging",false);
        _animator.SetBool("isDead",true);
        // !!! ADD API MANAGER IN START AND STARTCOROUTINE TO SAVEPROGRESS WITH CALLBACK METHOD !!!!
        Invoke(nameof(DisplayGameOverScreen), 2);
    }

    private void DisplayGameOverScreen()
    {
        gameOverUI.SetActive(true);
    }
}