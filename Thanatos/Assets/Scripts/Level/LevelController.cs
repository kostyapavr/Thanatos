using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static int aliveEnemies = 0;

    public static UnityEvent enemyDeathEvent = new UnityEvent();
    public static UnityEvent enemySpawnEvent = new UnityEvent();

    void Start()
    {
        enemyDeathEvent.AddListener(EnemyDeath);
        enemyDeathEvent.AddListener(EnemySpawned);
        DontDestroyOnLoad(this);
    }

    void EnemyDeath()
    {
        if (aliveEnemies > 0) aliveEnemies -= 1;
        else Debug.Log("Error: Negative aliveEnemies");
    }

    void EnemySpawned()
    {
        aliveEnemies += 1;
    }
}
