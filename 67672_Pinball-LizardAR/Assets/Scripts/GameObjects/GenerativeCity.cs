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
        city.Generate(100, Random.Range(0, 10000), 0.90f, gameObject);
        float leftRightDistance = city.GetDistanceBetweenPoints(0, 2);
        float topBottomDistance = city.GetDistanceBetweenPoints(1, 3);
        Bounds meshBounds = Base.GetComponent<MeshFilter>().mesh.bounds;
        float xLength = meshBounds.extents.x * 2;
        float zLength = meshBounds.extents.z * 2;
        Base.transform.localScale = new Vector3(leftRightDistance / xLength * 1.5f, 0.001f, topBottomDistance / zLength * 1.5f);
        yield return "done";
    }
}
