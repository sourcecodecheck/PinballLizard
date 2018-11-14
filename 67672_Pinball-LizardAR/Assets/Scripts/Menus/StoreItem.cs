using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour {

    public string ItemId;
    public int MayhemPrice;
    public int BugBucksPrice;

    private bool isSelected;
	// Use this for initialization
	void Start () {
        isSelected = false;
        StoreEvents.OnSelectItem += Select;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Select(string itemIdToSelect)
    {
        if(itemIdToSelect == ItemId)
        {
            if (isSelected)
            {
                
            }
            else
            {
                isSelected = true;
                GetComponentInChildren<Image>().color = new Color(1, 0, 1);
            }
        }
        else
        {
            isSelected = false;
            GetComponentInChildren<Image>().color = new Color(1, 1, 1);
        }
    }

    public void Purchase(string currencyToUse)
    {
        StoreEvents.SendPurchaseItem(ItemId, currencyToUse);
    }

    private void OnMouseDown()
    {
        StoreEvents.SendSelectItem(ItemId);
    }

    private void OnDestroy()
    {
        StoreEvents.OnSelectItem -= Select;
    }
}
