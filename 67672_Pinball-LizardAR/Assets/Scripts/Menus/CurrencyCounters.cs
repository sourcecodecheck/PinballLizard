using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyCounters : MonoBehaviour {

    public Inventory PlayerInventory;
    public Text MayhemCount;
    public Text BugBucksCount;
    public Text HolidayCurrencyCount;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MayhemCount.text = PlayerInventory.MayhemCount.ToString();
        BugBucksCount.text = PlayerInventory.BugBucksCount.ToString();
        HolidayCurrencyCount.text = PlayerInventory.HolidayCurrencyCount.ToString();
	}
}
