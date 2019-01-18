using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnEnemies : Pausable
{
    public GameObject Enemy;
    public float TimeToInitialSpawn;
    public float TimeToSpawnRepeat;
    public int ConcurrentEnemyCap;
    public float Scale;

    private List<GameObject> spawnedEnemies;
    private bool isSpawning;

    
    new void Start()
    {
        isSpawning = true;
        spawnedEnemies = new List<GameObject>();
        if (TimeToInitialSpawn > 0 && TimeToSpawnRepeat > 0)
        {
            InvokeRepeating("DoSpawn", TimeToInitialSpawn, TimeToSpawnRepeat);
        }
        AnimationEvents.OnMouthNommed += OnNom;
        AnimationEvents.OnHandsExited += OnArmsExit;
        base.Start();
    }

    
    void Update()
    {
        spawnedEnemies = spawnedEnemies.Where((e) => (e == null) == false).ToList();
    }

    private void DoSpawn()
    {
        if (!isPaused)
        {
            if (spawnedEnemies.Count < ConcurrentEnemyCap && isSpawning)
            {
                if (Enemy != null)
                {
                    Vector3 spawnOffset = new Vector3(Random.Range(-0.27f, 0.27f), 0, Random.Range(-0.27f, 0.27f));
                    GameObject spawnedEnemy = Instantiate(Enemy, gameObject.transform.position + spawnOffset, Quaternion.identity);
                    spawnedEnemy.transform.localScale *= Scale;
                    spawnedEnemy.transform.parent = gameObject.transform;
                    spawnedEnemies.Add(spawnedEnemy);
                }
            }
        }
    }

    private void OnNom()
    {
        isSpawning = false;
    }

    private void OnArmsExit()
    {
        isSpawning = true;
    }

    private new void OnDestroy()
    {
        AnimationEvents.OnMouthNommed -= OnNom;
        AnimationEvents.OnHandsExited -= OnArmsExit;
        base.OnDestroy();
    }
}
