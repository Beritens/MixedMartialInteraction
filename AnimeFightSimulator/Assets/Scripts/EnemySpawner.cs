using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    private int waveNumber = 0;
    private int enemiesToSpawn;
    private int enemiesRemaining;
    private float waveStartTime; // Time when the current wave started
    private float waveDuration;
    
    public static event Action<float> WaveDuration;
    public static event Action<int> enemiesToSpawnHandler;
    public static event Action WaveCompleted;
    public static event Action WaveStarted;
    public static event Action AllEnemiesDefeated;

    private bool waveActive = false;
    

    void Start()
    {
        StartNextWave();
    }

    private void Update()
    {
        waveDuration = Time.time - waveStartTime; // Calculate duration of the wave
        WaveDuration?.Invoke(waveDuration); 

    }

    void StartNextWave()
    {
        waveActive = true;
        waveNumber++;
        enemiesToSpawn = waveNumber;
        enemiesRemaining = enemiesToSpawn;
        enemiesToSpawnHandler?.Invoke(enemiesToSpawn);
        waveStartTime = Time.time; // Record start time of the wave

        WaveStarted?.Invoke();  // Announce that a new wave is starting
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Health enemyHealth = enemy.GetComponent<Health>();
        enemyHealth.onDeath += OnEnemyDefeated;
        
    }

    void OnEnemyDefeated(object sender, EventArgs empty)
    {
        enemiesRemaining--;
        if (!waveActive)
        {
            return;
        }
        if (enemiesRemaining <= 0)
        {
            waveActive = false;
            waveStartTime += waveDuration;
            StartCoroutine(WaitAndStartNextWave());
            WaveCompleted?.Invoke();
            AllEnemiesDefeated?.Invoke(); // Trigger the event when all enemies are defeated
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        
        StartNextWave();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 30), "Wave: " + waveNumber);
    }
}