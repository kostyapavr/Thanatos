using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Enemy[] enemyPrefabs;
    private List<int> usedInds;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) Debug.Log("No spawn points!");
        else if (enemyPrefabs == null || enemyPrefabs.Length == 0) Debug.Log("No enemies to spawn!");
        else SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < ResourceManager.Instance.enemiesToSpawn; i++)
        {
            int enemyInd = Random.Range(0, enemyPrefabs.Length);
            int spawnInd = RandomNoRepeat(spawnPoints.Length);
            Instantiate(enemyPrefabs[enemyInd], spawnPoints[spawnInd].position, Quaternion.identity);
            LevelController.enemySpawnEvent.Invoke();
        }
    }

    public int RandomNoRepeat(int max)
    {
        usedInds = new List<int>();
        int res = GenRandom(max);
        usedInds.Clear();
        return res;
    }

    private int GenRandom(int max)
    {
        int ind = Random.Range(0, max);
        int iter = 0;
        while (usedInds.Contains(ind))
        {
            if (iter > 20)
            {
                Debug.Log("Random gen error!");
                return 0;
            }
            ind = Random.Range(0, max);
            iter++;
        }
        usedInds.Add(ind);
        return ind;
    }
}
