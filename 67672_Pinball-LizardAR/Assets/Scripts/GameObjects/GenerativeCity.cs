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
    public GameObject Blank;
    public GameObject Base;
    public int NumberOfBuildingsGenerated;
    public float BuildingToBlankRatio;
    public int Seed;

    private List<GameObject> buildingBlock;
    private HexGrid city;
    void Start()
    {
        buildingBlock = new List<GameObject> { Building1, Building2, Building3, Building4 };
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
        Base.transform.position = transform.position;
        Base.transform.rotation = transform.rotation;
        city.Generate(NumberOfBuildingsGenerated, Random.Range(0, 10000), BuildingToBlankRatio, gameObject);
        float zeroTwoDistance = city.GetDistanceBetweenPoints(0, 2);
        float fiveThreeDistance = city.GetDistanceBetweenPoints(5, 3);
        float leftRightDistance = Mathf.Max(zeroTwoDistance, fiveThreeDistance);
        float topBottomDistance = city.GetDistanceBetweenPoints(1, 4);
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        //bounds extents are half the size so we multiply by 2
        float xLength = meshBounds.extents.x * 2.0f; 
        float zLength = meshBounds.extents.z * 2.0f;
        Base.transform.localScale = new Vector3(leftRightDistance  / xLength  , 0.001f, topBottomDistance / zLength );
        yield return "done";
    }
}
