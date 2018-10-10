using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject IceEnemy;
    public GameObject FireEnemy;
    public GameObject AtomEnemy;
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
            if (IceEnemy != null)
            {
                GameObject spawnedEnemy = null;
                if (FireEnemy != null)
                {
                    if (AtomEnemy != null)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                spawnedEnemy = Instantiate(IceEnemy, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                            case 1:
                                spawnedEnemy = Instantiate(FireEnemy, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                            case 2:
                                spawnedEnemy = Instantiate(AtomEnemy, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                        }

                    }
                    else
                    {
                        switch (Random.Range(0, 1))
                        {
                            case 0:
                                spawnedEnemy = Instantiate(IceEnemy, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                            case 1:
                                spawnedEnemy = Instantiate(FireEnemy, gameObject.transform.parent.position, Quaternion.identity);
                                break;
                        }
                    }
                }
                else
                {
                    spawnedEnemy = Instantiate(IceEnemy, gameObject.transform.parent.position, Quaternion.identity);
                }
                spawnedEnemy.transform.parent = gameObject.transform;
                ++enemyCount;
            }
        }
    }
}
