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

    private List<GameObject> storeItems;
    // Use this for initialization
    void Start()
    {
        storeItems = new List<GameObject>();
        StoreEvents.OnLoadItem += LoadItem;
        StoreEvents.SendLoadStore(StoreId, CatalogVersion);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadItem(string itemId, int mayhemPrice, int bugBucksPrice)
    {
        storeItems = storeItems.Where((item) => item != null).ToList();
        GameObject storeItem = Instantiate(StoreItem, ItemParent.transform);
        StoreItem itemScript = storeItem.GetComponent<StoreItem>();
        itemScript.ItemId = itemId;
        itemScript.MayhemPrice = mayhemPrice;
        itemScript.BugBucksPrice = bugBucksPrice;
        Text[] texts = storeItem.GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            if(text.name.ToLower().Contains("item"))
            {
                text.text = itemId;
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

    private void OnDestroy()
    {
        StoreEvents.OnLoadItem -= LoadItem;
        foreach(GameObject storeItem in storeItems)
        {
            Destroy(storeItem);
        }
    }
}
