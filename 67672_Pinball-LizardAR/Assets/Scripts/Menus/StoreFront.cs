using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoreFront : MonoBehaviour
{
    public string StoreId;
    public string CatalogVersion;
    public GameObject StoreItem;
    public GameObject ItemParent;
    public GameObject PurchaseButtons;

    private List<GameObject> storeItems;
    // Use this for initialization
    void Start()
    {
        storeItems = new List<GameObject>();
        StoreEvents.OnLoadStoreItem += LoadItem;
        StoreEvents.OnShowPurchaseButton += ShowPurchaseButton;
        StoreEvents.SendLoadStore(StoreId, CatalogVersion);
        PurchaseButtons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadItem(string itemId, string itemName, int mayhemPrice, int bugBucksPrice, string storeId, string bugBucksKey, string mayhemKey)
    {
        storeItems = storeItems.Where((item) => item != null).ToList();
        GameObject storeItem = Instantiate(StoreItem, ItemParent.transform);
        StoreItem itemScript = storeItem.GetComponent<StoreItem>();
        itemScript.ItemId = itemId;
        itemScript.MayhemPrice = mayhemPrice;
        itemScript.BugBucksPrice = bugBucksPrice;
        itemScript.CatalogVersion = CatalogVersion;
        itemScript.StoreId = storeId;
        itemScript.BugBucksKey = bugBucksKey;
        itemScript.MayhemKey = mayhemKey;
        StoreId = storeId;
        Text[] texts = storeItem.GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            if(text.name.ToLower().Contains("item"))
            {
                text.text = itemName;
            }
            else if (text.name.ToLower().Contains("may"))
            {
                text.text = mayhemPrice.ToString();
            }
            else if (text.name.ToLower().Contains("bug"))
            {
                text.text = bugBucksPrice.ToString();
            }
        }
        storeItem.transform.position += new Vector3(150 * storeItems.Count, 0);
        storeItems.Add(storeItem);
    }
    public void ShowPurchaseButton(bool isShowing)
    {
        PurchaseButtons.SetActive(isShowing);
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadStoreItem -= LoadItem;
        StoreEvents.OnShowPurchaseButton -= ShowPurchaseButton;
        foreach (GameObject storeItem in storeItems)
        {
            Destroy(storeItem);
        }
    }
}
