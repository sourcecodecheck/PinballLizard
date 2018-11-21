using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public string ItemId;
    public string ItemName;
    public int Count;
    // Use this for initialization
    void Awake()
    {
        Count = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {

    }
}
