using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemies : Pausable
{
    public GameObject Enemy;
    public float TimeToInitialSpawn;
    public float TimeToSpawnRepeat;
    public int ConcurrentEnemyCap;
    public int TotalSpawnCap;
    public float Scale;
    public float CameraOffset;
    public float HeightOffset;
    public float[] Rotations;

    private List<GameObject> spawnedEnemies;
    private bool isSpawning;
    private float[] heightOffsets;
    private int totalSpawned;
    
    new void Start()
    {
        isSpawning = false;
        spawnedEnemies = new List<GameObject>();
        if (TimeToInitialSpawn > 0 && TimeToSpawnRepeat > 0)
        {
            InvokeRepeating("DoSpawn", TimeToInitialSpawn, TimeToSpawnRepeat);
        }
        AnimationEvents.OnMouthEntered += OnMouthEntered;
        heightOffsets = new float[] { 0, HeightOffset, HeightOffset * 0.5f };
        totalSpawned = 0;
        base.Start();
    }

    
    void Update()
    {
    }


    private void DoSpawn()
    {
        if (!isPaused)
        {
            if (spawnedEnemies.Count < ConcurrentEnemyCap && isSpawning)
            {
                if (Enemy != null)
                {
                    Vector3 offset = new Vector3(0, heightOffsets[spawnedEnemies.Count], CameraOffset);
                    GameObject spawnedEnemy = Instantiate(Enemy, gameObject.transform.position  + offset, Quaternion.identity, gameObject.transform);
                    spawnedEnemy.transform.localScale *= Scale;
                    spawnedEnemy.GetComponent<EnemyBehavior>().Rotation = Rotations[Random.Range(0, Rotations.Length - 1)];
                    spawnedEnemies.Add(spawnedEnemy);
                    ++totalSpawned;
                }
            }
        }
    }

    private void OnNom()
    {
        isSpawning = false;
        foreach(GameObject enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

    private void OnMouthEntered()
    {
        isSpawning = true;
    }

    private new void OnDestroy()
    {
        AnimationEvents.OnMouthEntered -= OnMouthEntered;
        base.OnDestroy();
    }
}
