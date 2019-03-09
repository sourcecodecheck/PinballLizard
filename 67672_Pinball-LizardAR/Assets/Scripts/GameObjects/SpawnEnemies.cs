using UnityEngine;
using System.Linq;
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
    public float[] HeightOffsets;
    public float[] Rotations;

    private List<GameObject> spawnedEnemies;
    private bool isSpawning;
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
        totalSpawned = 0;
        base.Start();
    }

    
    void Update()
    {
        spawnedEnemies = spawnedEnemies.Where((enemy) => enemy != null).ToList();
    }


    private void DoSpawn()
    {
        if (!isPaused)
        {
            if (spawnedEnemies.Count < ConcurrentEnemyCap && isSpawning && totalSpawned < TotalSpawnCap)
            {
                if (Enemy != null)
                {
                    int offsetIndex = spawnedEnemies.Count;
                    while(offsetIndex > HeightOffsets.Length - 1)
                    {
                        offsetIndex -= HeightOffsets.Length;
                    }
                    Vector3 offset = new Vector3(0, HeightOffsets[offsetIndex], CameraOffset);
                    GameObject spawnedEnemy = Instantiate(Enemy, gameObject.transform.position  + offset, Quaternion.identity, gameObject.transform);
                    spawnedEnemy.transform.localScale *= Scale;
                    float rotation = Rotations[Random.Range(0, Rotations.Length - 1)];
                    if(rotation > 0)
                    {
                        spawnedEnemy.transform.Rotate(new Vector3(0, 1, 0), 90);
                    }
                    else
                    {
                        spawnedEnemy.transform.Rotate(new Vector3(0, 1, 0), -90);
                    }
                    spawnedEnemy.GetComponent<EnemyBehavior>().Rotation = rotation;
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
