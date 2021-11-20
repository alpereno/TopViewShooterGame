using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Wave[] waves;

    Wave currentWave;
    int currentWaveNumber;
    int enemiesRemainingToSpawn;                //keep track how many enemies are remaining to spawn
    float nextSpawnTime;
    float enemiesRemainingAlive;

    private void Start()
    {
        nextWave();
    }

    private void Update()
    {
        //basicly on a timer spawn however many enemies are in our current wave
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.onDeath += onEnemyDeath;
        }
    }

    void onEnemyDeath() {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            nextWave();
        }
    }

    void nextWave() {
        currentWaveNumber++;
        print("Wave: " + currentWaveNumber);
        if (currentWaveNumber-1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave {
        //information about each wave... like how many enemies are in a wave, spawn rate each wave...
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
