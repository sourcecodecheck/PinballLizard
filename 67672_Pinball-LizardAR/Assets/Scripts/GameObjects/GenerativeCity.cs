using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.DataStructures;

public class GenerativeCity : MonoBehaviour
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
    public int Seed;

    private List<GameObject> buildingBlock;
    private HexGrid city;

    void Start()
    {
        buildingBlock = new List<GameObject> { Building1, Building2, Building3, Building4,
            Building5,  Building6,  Building7, Building8 };
        city = new HexGrid();
        StartCoroutine(BuildCity());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BuildCity()
    {
        city.BuildingObjects = buildingBlock;
        city.BlankSpot = Blank;
        city.Generate(NumberOfBuildingsGenerated, Random.Range(0, 10000), BuildingToBlankRatio, gameObject);
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        float xLength = meshBounds.extents.x;
        float zLength = meshBounds.extents.z ;

        Base.transform.localScale = new Vector3(city.XDistance / xLength * 1.25f, 0.001f, city.ZDistance / zLength * 1.15f);
        Base.transform.rotation = city.GetRootRotation();
        Base.transform.position = city.GetRootPosition() + new Vector3(0, -0.01f, 0);
        Base.transform.parent = gameObject.transform.parent;
        yield return "done";
    }
}
