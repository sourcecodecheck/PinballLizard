using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Pausable
{
    //protoypes to be instanced
    public GameObject[] Buildings;
    public GameObject Blank;
    public GameObject DaBomb;
    //actual instance of an object
    public GameObject Base;

    public int NumberOfBuildingsGenerated;
    public float BuildingToBlankRatio;
    public float scale;
    public int Seed;
    public bool isAR;

    private const float rotateSpeed = 10f;

    private List<GameObject> buildingBlock;
    private HexGrid city;
    private GameObject daBombInstance;

    new void Start()
    {
        base.Start();
        GamePlayEvents.OnBombDetonated += DaBombAnimation;
        buildingBlock = new List<GameObject>(Buildings);
        city = new HexGrid();
        StartCoroutine(BuildCity());
    }

    void Update()
    {
        if (!isAR && !isPaused)
        {
            transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
        }
    }

    IEnumerator BuildCity()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.ChallengeModeSet) && PlayerPrefs.GetInt(PlayerPrefsKeys.ChallengeModeSet) == 1)
        {
            GenerateCityChallenge();
        }
        else
        {
            GenerateCityPlayerLevel();
        }

        if (isAR)
        {
            ARScaling();
        }
        else
        {
            NonARScaling();
        }
        AnimationEvents.SendMouthEnter();
        yield return "done";
    }
    void GenerateCityChallenge()
    {
        int seedFromServer = PlayerPrefs.GetInt(PlayerPrefsKeys.DailyChallengeSeed);
        float average = 0f;
        for (int i = 1; i < 9; ++i)
        {
            average += seedFromServer % 10;
            seedFromServer /= 10;
        }
        average *= 0.11111111111111111111111111111111f;
        float ratio = average - (float)System.Math.Truncate(average);
        float ratioClamped = Mathf.Max(0.65f, ratio);
        Mathf.Lerp(30, 120, seedFromServer * 0.00000001f);
        city.BuildingObjects = buildingBlock;
        city.BlankSpot = Blank;
        int buildingsGenerated = city.Generate(NumberOfBuildingsGenerated, seedFromServer, ratioClamped, gameObject);
        TrackingEvents.SendCityGenerated(buildingsGenerated);
    }

    void GenerateCityPlayerLevel()
    {
        city.BuildingObjects = buildingBlock;
        city.BlankSpot = Blank;
        int buildingsGenerated = city.Generate(NumberOfBuildingsGenerated,
            UnityEngine.Random.Range(0, 10000), BuildingToBlankRatio, gameObject);
        TrackingEvents.SendCityGenerated(buildingsGenerated);
    }

    void ARScaling()
    {
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        float xLength = meshBounds.extents.x * 0.5f;
        float zLength = meshBounds.extents.z * 0.5f;
        Base.transform.rotation = city.GetRootRotation();
        Base.transform.position = city.GetRootPosition() + new Vector3(0, -0.025f, 0);
        float xScale = city.XDistance / xLength;
        float zScale = city.ZDistance / zLength;
        Base.transform.localScale = new Vector3(xScale * 0.9f, 0.0005f, zScale * 0.7f);
        transform.localScale *= scale;
        Base.transform.parent = gameObject.transform.parent;
    }

    void NonARScaling()
    {
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        float xLength = meshBounds.extents.x * 0.001f;
        float zLength = meshBounds.extents.z * 0.001f;
        Base.transform.rotation = city.GetRootRotation();
        Base.transform.position = city.GetRootPosition() + new Vector3(0, -1.0f, 0);
        Base.transform.parent = null;
        float xScale = city.XDistance / xLength;
        float zScale = city.ZDistance / zLength;
        Base.transform.localScale = new Vector3(xScale * 0.7f, 0.1f, zScale * 0.5f);
        transform.localScale *= scale;
        Base.transform.parent = gameObject.transform;
    }

    void DaBombAnimation()
    {
        daBombInstance = Instantiate(DaBomb, transform);
        Invoke("DestroyDaBomb", 5f);
    }

    void DestroyDaBomb()
    {
        Destroy(daBombInstance);
        daBombInstance = null;
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        GamePlayEvents.OnBombDetonated -= DaBombAnimation;
    }
}
