using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.DataStructures;

public class City : Pausable
{
    public GameObject Building1;
    public GameObject Building2;
    public GameObject Building3;
    public GameObject Building4;
    public GameObject Building5;
    public GameObject Building6;
    public GameObject Building7;
    public GameObject Building8;
    public GameObject Blank;
    public GameObject Base;
    public int NumberOfBuildingsGenerated;
    public float BuildingToBlankRatio;
    public float scale;
    public int Seed;
    public bool isAR;

    private List<GameObject> buildingBlock;
    private HexGrid city;

    new void Start()
    {
        base.Start();
        buildingBlock = new List<GameObject> { Building1, Building2, Building3, Building4,
            Building5,  Building6,  Building7, Building8 };
        city = new HexGrid();
        StartCoroutine(BuildCity());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAR && !isPaused)
        {
            transform.Rotate(new Vector3(0, 1, 0), 10 * Time.deltaTime);
        }
    }

    IEnumerator BuildCity()
    {
        if(PlayerPrefs.HasKey("ischallenge") && PlayerPrefs.GetInt("ischallenge") == 1)
        {
            GenerateCityChallenge();
        }
        else
        {
            GenerateCityPlayerLevel();
        }
        
        if(isAR)
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
        int seedFromServer = PlayerPrefs.GetInt("dailychallenge");
        float average = 0f;
        for(int i = 1; i < 9; ++i)
        {
            average += seedFromServer % 10;
            seedFromServer = 10;
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
        int buildingsGenerated = city.Generate(NumberOfBuildingsGenerated, Random.Range(0, 10000), BuildingToBlankRatio, gameObject);
        TrackingEvents.SendCityGenerated(buildingsGenerated);
    }

    void ARScaling()
    {
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        float xLength = meshBounds.extents.x * 0.5f;
        float zLength = meshBounds.extents.z * 0.5f;
        Base.transform.rotation = city.GetRootRotation();
        Base.transform.position = city.GetRootPosition() + new Vector3(0, -0.1f, 0);
        Base.transform.localScale = new Vector3(city.XDistance / xLength, 0.001f, city.ZDistance / zLength);
        transform.localScale *= scale;
        Base.transform.parent = gameObject.transform.parent;
    }

    void NonARScaling()
    {
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        float xLength = meshBounds.extents.x * 0.0005f;
        float zLength = meshBounds.extents.z * 0.0005f;
        Base.transform.rotation = city.GetRootRotation();
        Base.transform.position = city.GetRootPosition() + new Vector3(0, -1.0f, 0);
        Base.transform.parent = null;
        Base.transform.localScale = new Vector3(city.XDistance / xLength, 0.1f, city.ZDistance / zLength);
        transform.localScale *= scale;
        Base.transform.parent = gameObject.transform;
    }
    new void OnDestroy()
    {
        base.OnDestroy();
    }
}
