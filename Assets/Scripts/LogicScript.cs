using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    private double currSpeedIncrement = 0;
    private float currSpawnRateDecrement = 0;
    private const float defaultSpawnRate = 2f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverScreen;
    public BirdScript bird;
    public PipeMoveScript pipeMove;
    public AudioSource scoreUp;
    public AudioSource music;

    private List<PipeMoveScript> pipes = new List<PipeMoveScript>();

    [ContextMenu("Increase")]
    public void addScore(int scoreToAdd) {
        if (bird.birdIsAlive)
        {
            playerScore += scoreToAdd;
            scoreUp.Play();
            scoreText.text = playerScore.ToString();
            if (playerScore % 5 == 0) {
                IncreaseSpeedSpawned();
            }
        }
    }

    public void startGame()
    {
        LoadHighScore();
        SceneManager.LoadScene("Game");
    }

    public void restartGame()
    {
        currSpeedIncrement = 0;
        currSpawnRateDecrement = 0;
        ResetAllPipes();

        PipeSpawnScript pipeSpawner = FindObjectOfType<PipeSpawnScript>();
        if (pipeSpawner != null)
        {
            pipeSpawner.UpdateSpawnRate(defaultSpawnRate);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        SaveHighScore();
        finalScoreText.text = "Your Score: " + playerScore.ToString();
        highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        gameOverScreen.SetActive(true);
        StopAllPipes();
    }

    private void SaveHighScore()
    {
        // Get the current high score from PlayerPrefs
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0); // Default to 0 if no high score exists

        // Check if the current score is higher than the saved high score
        if (playerScore > currentHighScore)
        {
            // Save the new high score
            PlayerPrefs.SetInt("HighScore", playerScore);
            PlayerPrefs.Save(); // Save changes to disk
            Debug.Log("New High Score Saved: " + playerScore);
        }
    }

    private void LoadHighScore()
    {
        // Get the high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0); // Default to 0 if no high score exists

        // Display the high score (optional)
        Debug.Log("Loaded High Score: " + highScore);
    }

    public void ResetHighScore()
    {
        // Reset the high score to 0
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save(); // Save changes to disk

        // Update the UI (if applicable)
        if (highScoreText != null)
        {
            highScoreText.text = "Highscore: 0";
        }

        Debug.Log("High Score Reset");
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

    private void IncreaseSpeedSpawned()
    {
        if (currSpeedIncrement < 5) {
            currSpeedIncrement += 0.25;
        }
        if (currSpawnRateDecrement < 1) {
            currSpawnRateDecrement += 0.05f;
        }
        foreach (var pipe in pipes)
        {
            pipe.moveSpeed += 0.25;
        }

        PipeSpawnScript pipeSpawner = FindObjectOfType<PipeSpawnScript>();
        if (pipeSpawner != null)
        {
            pipeSpawner.UpdateSpawnRate(defaultSpawnRate - currSpawnRateDecrement);
        }
    }

    public double GetCurrSpeedIncrement() {
        return currSpeedIncrement;
    }

    public float GetCurrSpawnRateDecrement() {
        return currSpawnRateDecrement;
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