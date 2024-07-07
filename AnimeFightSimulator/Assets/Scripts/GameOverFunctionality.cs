using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameOverFunctionality : MonoBehaviour
{
    public float minWaveTime = 30f; // Maximum time allowed per wave
    private float remainingTime; // Time remaining for the current wave
    private bool isWaveActive = true; // State to track if the wave is active
    private bool isGameOver = false; // Flag to indicate game over state
    public int timePerEnemy = 10;
    private bool timerActive = true;

    void Start()
    {
        remainingTime = minWaveTime;
        EnemySpawner.WaveCompleted += ResetWaveTimer;
        EnemySpawner.WaveStarted += ResetAndStartWaveTimer; // Reset and start the timer when a new wave starts
        EnemySpawner.enemiesToSpawnHandler += UpdateRemainingTimeForNextWave;
        EnemySpawner.AllEnemiesDefeated += PauseTimer; // Subscribe to pause the timer
    }

    void Update()
    {
        if (isWaveActive && timerActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                GameOver();
            }
        }
    }

    void OnGUI()
    {
        if (isWaveActive)
        {
            // Remaining Time Stuff
            GUI.Label(new Rect(10, 30, 250, 20), $"Time Remaining: {remainingTime:F2} seconds");
        }
        else if (isGameOver)
        {
            // Style for "GAME OVER"
            GUIStyle gameOverStyle = new GUIStyle(GUI.skin.label);
            gameOverStyle.fontSize = 30; // Set the font size
            gameOverStyle.normal.textColor = Color.red; // Set the text color
            gameOverStyle.fontStyle = FontStyle.Bold; // Make the font bold
            gameOverStyle.alignment = TextAnchor.MiddleCenter; // Center the text

            // Style for "THE BURB HAS CONSUMED YOU"
            GUIStyle detailsStyle = new GUIStyle(GUI.skin.label);
            detailsStyle.fontSize = 18; // Set a smaller font size
            detailsStyle.normal.textColor = Color.white; // Set a different text color
            detailsStyle.alignment = TextAnchor.MiddleCenter; // Center the text

            // Define the rects for the labels to center them on the screen
            Rect gameOverRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 60, 300, 50);
            Rect detailsRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 300, 50);

            // Use the GUIStyle to draw each label
            GUI.Label(gameOverRect, "GAME OVER", gameOverStyle);
            GUI.Label(detailsRect, "THE BURB HAS CONSUMED YOU", detailsStyle);
        }
    }
    
    
    void PauseTimer()
    {
        timerActive = false; // Pause the timer
    }
    
    void ResetWaveTimer()
    {
        isWaveActive = true;
        timerActive = true; // Resume the timer for the new wave
    }
    
    void ResetAndStartWaveTimer()
    {
        //remainingTime = maxWaveTime; // Reset the remaining time to the max wave time
        timerActive = true; // Ensure the timer is active
    }
    
    void UpdateRemainingTimeForNextWave(int enemiesCount)
    {
        remainingTime = minWaveTime + (timePerEnemy * enemiesCount); // Reset the timer based on the number of enemies in the next wave
    }

    void GameOver()
    {
        isWaveActive = false;
        isGameOver = true;
        Time.timeScale = 0;
        
        StartCoroutine(RestartSceneAfterDelay(5));
        
    }
    
    IEnumerator RestartSceneAfterDelay(float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSecondsRealtime(delay);

        // Reset the timescale, if the game was paused
        Time.timeScale = 1;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        //EnemySpawner.WaveDuration -= CheckWaveTime;
        EnemySpawner.WaveCompleted -= ResetWaveTimer;
        EnemySpawner.WaveStarted -= ResetAndStartWaveTimer;
        EnemySpawner.enemiesToSpawnHandler -= UpdateRemainingTimeForNextWave;
        EnemySpawner.AllEnemiesDefeated -= PauseTimer;
    }
}