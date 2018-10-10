using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public float TimeToInitialSpawn;
    public float TimeToSpawnRepeat;
    public int NumberToSpawnTotal;

    private int enemyCount;
    // Use this for initialization
    void Start()
    {
        enemyCount = 0;
        if (TimeToInitialSpawn > 0 && TimeToSpawnRepeat > 0)
        {
            InvokeRepeating("DoSpawn", TimeToInitialSpawn, TimeToSpawnRepeat);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DoSpawn()
    {
        if (enemyCount < NumberToSpawnTotal)
        {
            //faster execute time written this way
            if (Enemy1 != null)
            {
                GameObject spawnedEnemy = null;
                if (Enemy2 != null)
                {
                    if (Enemy3 != null)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                spawnedEnemy = Instantiate(Enemy1, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                            case 1:
                                spawnedEnemy = Instantiate(Enemy2, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                            case 2:
                                spawnedEnemy = Instantiate(Enemy3, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                        }

                    }
                    else
                    {
                        switch (Random.Range(0, 1))
                        {
                            case 0:
                                spawnedEnemy = Instantiate(Enemy1, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                            case 1:
                                spawnedEnemy = Instantiate(Enemy2, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                        }
                    }
                }
                else
                {
                    spawnedEnemy = Instantiate(Enemy1, gameObject.transform.parent.position, Quaternion.identity);
                }
                spawnedEnemy.transform.parent = gameObject.transform;
                ++enemyCount;
            }
        }
    }
}
