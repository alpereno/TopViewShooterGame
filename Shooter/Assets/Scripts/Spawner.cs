using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public event System.Action onNewWave;
    public event System.Action<int> onNewWave;
    public bool devMode;
    public Wave[] waves;
    [SerializeField] private Enemy enemyPrefab;

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
        if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.onDeath += onEnemyDeath;
            spawnedEnemy.setEnemyProperties(currentWave.enemyHealth, currentWave.enemyMovementSpeed, currentWave.hitsNumberToKillPlayer,
                currentWave.enemySkinColor);
        }

        if (devMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }
                nextWave();
            }
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

            if (onNewWave != null)
            {
                onNewWave(currentWaveNumber);                   // u can use without parameter but remember delete <int> part up there
            }                                                   // arrange the player health with this event and change map

            //if (onNewWave != null)
            //{
            //    onNewWave();
            //}
        }
    }

    [System.Serializable]
    public class Wave {
        // last wave should be infinite so the game never finish
        public bool infinite;
        //information about each wave... like how many enemies are in a wave, spawn rate each wave...
        public int enemyCount;
        public float timeBetweenSpawns;
        public float enemyHealth;
        public float enemyMovementSpeed;
        public float hitsNumberToKillPlayer;
        public Color enemySkinColor;
     // public float angularSpeed;              // turning speed
     // public float acceleration;              // if u open this comment line you have to arrange the setEnemyProperties method which is inside
                                                // of Enemy script.
    }
}
