using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    public int count=10;
    public GameObject spawnPrefab;
    private void OnDestroy()
    {
        for(int i = 0; i < count; i++)
        {
            Vector3 pos = getMovementDirection();
            GameObject enemy= Instantiate(spawnPrefab, transform.position+pos*(i-count/2)*0.5f, transform.rotation);
            enemy.GetComponent<Enemy>().source = source;
            enemy.GetComponent<Enemy>().SetCurrentPathIndex(getCurrentPathIndex());
        }
    }
}
