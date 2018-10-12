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
    private List<GameObject> buildingBlock;
    private HexGrid city;
    void Start()
    {
        buildingBlock = new List<GameObject> { Building1, Building2, Building3, Building4};
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
        city.Generate(100, (int)(Random.value * 100), 0.90f, gameObject);
        yield return "done";
    }
}
