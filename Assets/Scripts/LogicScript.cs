using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public GameObject gameOverScreen;
    public BirdScript bird;
    public PipeMoveScript pipeMove;

    private List<PipeMoveScript> pipes = new List<PipeMoveScript>();

    [ContextMenu("Increase")]
    public void addScore(int scoreToAdd) {
        if (bird.birdIsAlive)
        {
            playerScore += scoreToAdd;
            scoreText.text = playerScore.ToString();
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        finalScoreText.text = "Your Score: " + playerScore.ToString();
        gameOverScreen.SetActive(true);
        StopAllPipes();
    }

    public void RegisterPipe(PipeMoveScript pipe)
    {
        pipes.Add(pipe);
    }

    private void StopAllPipes()
    {
        foreach (var pipe in pipes)
        {
            pipe.moveSpeed = 0;
        }
    }

    private void ResetAllPipes()
    {
        foreach (var pipe in pipes)
        {
            pipe.moveSpeed = pipe.initialMoveSpeed;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            ResetAllPipes();
        }
    }
}