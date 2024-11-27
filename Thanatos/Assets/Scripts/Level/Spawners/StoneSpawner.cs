using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] stonePrefabs;
    protected List<int> usedInds = new List<int>();

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) Debug.Log("No spawn points!");
        else if (stonePrefabs == null || stonePrefabs.Length == 0) Debug.Log("No stones to spawn!");
        else Invoke("SpawnStones", 0.02f);
    }

    void SpawnStones()
    {
        for (int i = 0; i < ResourceManager.Instance.rocksToSpawn; i++)
        {
            int stoneInd = Random.Range(0, stonePrefabs.Length);
            int spawnInd = RandomNoRepeat(spawnPoints.Length);
            Instantiate(stonePrefabs[stoneInd], spawnPoints[spawnInd].position, Quaternion.identity);
        }
    }

    bool isOccupied(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 3.0f);
        if (colliders.Length == 0) return false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() || collider.GetComponent<Player>()) return true;
        }
        return false;
    }

    public int RandomNoRepeat(int max)
    {
        usedInds = new List<int>();
        return GenRandom(max);
    }

    protected int GenRandom(int max)
    {
        int ind = Random.Range(0, max);
        int iter = 0;
        while (usedInds.Contains(ind) || isOccupied(spawnPoints[ind].position))
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
