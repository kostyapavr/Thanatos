using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    private List<int> usedInds;

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
