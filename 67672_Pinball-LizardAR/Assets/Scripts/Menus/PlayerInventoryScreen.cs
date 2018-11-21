using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerInventoryScreen : MonoBehaviour {

    public string CatalogVersion;
    public GameObject InventoryItem;
    public Inventory PlayerInventory;

    private Dictionary<string, InventoryItem> inventory;
    // Use this for initialization
    void Awake () {
        inventory = new Dictionary<string, InventoryItem>();
        StoreEvents.OnLoadInventoryItem += LoadInventoryItem;
        StoreEvents.SendLoadInventory(CatalogVersion);
        GetComponentsInChildren<Text>().Where((text) => text.name == "BugsEaten").First().text = "BugsEaten: " + PlayerInventory.BugsEatenCount;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadInventoryItem(string itemId, string itemName, int count)
    {
            GameObject inventoryItemObject = Instantiate(InventoryItem, gameObject.transform);
            inventoryItemObject.transform.position += new Vector3(200 * inventory.Count, 0);
            InventoryItem inventoryItemScript = inventoryItemObject.GetComponent<InventoryItem>();
            inventory.Add(itemId, inventoryItemScript);
            inventoryItemScript.ItemName = itemName;
            
            inventory[itemId].GetComponentInChildren<Text>().text = itemName + ": " + count.ToString();
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadInventoryItem -= LoadInventoryItem;
    }
}
