using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public string ItemId;
    public string StoreId;
    public string CatalogVersion;
    public int MayhemPrice;
    public int BugBucksPrice;
    public string MayhemKey;
    public string BugBucksKey;

    private bool isSelected;
    // Use this for initialization
    void Start()
    {
        isSelected = false;
        StoreEvents.OnSelectItem += Select;
        StoreEvents.OnStartPurchase += Purchase;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Select(string itemIdToSelect)
    {
        if (itemIdToSelect == ItemId)
        {
            isSelected = true;
            GetComponentInChildren<Image>().color = new Color(1, 0, 1);
            StoreEvents.SendShowPurchaseButton(true);
        }
        else
        {
            isSelected = false;
            GetComponentInChildren<Image>().color = new Color(1, 1, 1);
        }
    }

    public void Purchase(string currencyToUse)
    {
        if (isSelected)
        {
            int price = 0;
            if(currencyToUse ==  MayhemKey)
            {
                price = MayhemPrice;
            }
            else if( currencyToUse == BugBucksKey)
            {
                price = BugBucksPrice;
            }
            if (price > 0)
            {
                StoreEvents.SendPurchaseItem(ItemId, currencyToUse, CatalogVersion, StoreId, price);
            }
            StoreEvents.SendSelectItem("");
            StoreEvents.SendShowPurchaseButton(false);
        }
    }

    public void OnClick()
    {
        StoreEvents.SendSelectItem(ItemId);
    }

    private void OnDestroy()
    {
        StoreEvents.OnSelectItem -= Select;
        StoreEvents.OnStartPurchase -= Purchase;
    }
}
