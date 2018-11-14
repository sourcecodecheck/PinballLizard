using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject IceEnemy;
    public GameObject FireEnemy;
    public GameObject AtomEnemy;
    public float TimeToInitialSpawn;
    public float TimeToSpawnRepeat;
    public int NumberToSpawnTotal;
    public int ConcurrentEnemyCap;

    private int enemyCount;
    private List<GameObject> spawnedEnemies;

    // Use this for initialization
    void Start()
    {
        spawnedEnemies = new List<GameObject>();
        enemyCount = 0;
        if (TimeToInitialSpawn > 0 && TimeToSpawnRepeat > 0)
        {
            InvokeRepeating("DoSpawn", TimeToInitialSpawn, TimeToSpawnRepeat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnedEnemies = spawnedEnemies.Where((e) => (e == null) == false).ToList();
    }

    private void DoSpawn()
    {
        if (enemyCount < NumberToSpawnTotal && spawnedEnemies.Count < ConcurrentEnemyCap)
        {
            //faster execute time written this way than by checking for each individual one.
            if (IceEnemy != null)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-0.27f, 0.27f), 0, Random.Range(-0.27f, 0.27f));
                GameObject spawnedEnemy = null;
                if (FireEnemy != null)
                {
                    if (AtomEnemy != null)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                spawnedEnemy = Instantiate(IceEnemy, gameObject.transform.parent.position + spawnOffset, Quaternion.identity);
                                break;
                            case 1:
                                spawnedEnemy = Instantiate(FireEnemy, gameObject.transform.parent.position + spawnOffset, Quaternion.identity);
                                break;
                            case 2:
                                spawnedEnemy = Instantiate(AtomEnemy, gameObject.transform.parent.position + spawnOffset, Quaternion.identity);
                                break;
                        }
                    }
                    else
                    {
                        switch (Random.Range(0, 1))
                        {
                            case 0:
                                spawnedEnemy = Instantiate(IceEnemy, gameObject.transform.parent.position + spawnOffset, Quaternion.identity);
                                break;
                            case 1:
                                spawnedEnemy = Instantiate(FireEnemy, gameObject.transform.parent.position + spawnOffset, Quaternion.identity);
                                break;
                        }
                    }
                }
                else
                {
                    spawnedEnemy = Instantiate(IceEnemy, gameObject.transform.parent.position + spawnOffset, Quaternion.identity);
                }
                spawnedEnemy.transform.parent = gameObject.transform;
                spawnedEnemies.Add(spawnedEnemy);
                ++enemyCount;
            }
        }
    }
}
