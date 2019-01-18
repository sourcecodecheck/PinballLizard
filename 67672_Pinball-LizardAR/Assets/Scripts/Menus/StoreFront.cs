using UnityEngine;

public class StoreFront : MonoBehaviour
{
    public string StoreId;
    public string CatalogVersion;
    public StoreItem SpicyMeatABall;
    public StoreItem DaBomb;
    public StoreItem StandardChest;
    public StoreItem EpicChest;
    public StoreItem ArachnoFeast;
    public Inventory PlayerInventory;
    
    void Start()
    {
        StoreEvents.OnLoadStoreItem += LoadItem;
        StoreEvents.SendLoadStore(StoreId, CatalogVersion);
        if(PlayerPrefs.HasKey(PlayerPrefsKeys.EventSet) &&
            PlayerPrefs.GetInt(PlayerPrefsKeys.EventSet) == 1)
        {
            ArachnoFeast.SetButtonActive(true);
        }
        else
        {
            ArachnoFeast.SetButtonActive(false);
        }

        SpicyMeatABall.PlayerInventory = PlayerInventory;
        DaBomb.PlayerInventory = PlayerInventory;
        StandardChest.PlayerInventory = PlayerInventory;
        EpicChest.PlayerInventory = PlayerInventory;
        ArachnoFeast.PlayerInventory = PlayerInventory;
    }

    
    void Update()
    {

    }

    private void LoadItem(StoreItemData itemData)
    {
        string lowerCasedItemId = itemData.ItemId.ToLower();
        if(lowerCasedItemId.Contains(SpicyMeatABall.KeyTerm))
        {
            SpicyMeatABall.ItemData = itemData;
        }
        else if (lowerCasedItemId.Contains(DaBomb.KeyTerm))
        {
            DaBomb.ItemData = itemData;
        }
        else if (lowerCasedItemId.Contains(StandardChest.KeyTerm))
        {
            StandardChest.ItemData = itemData;
        }
        else if (lowerCasedItemId.Contains(EpicChest.KeyTerm))
        {
            EpicChest.ItemData = itemData;
        }
        else if (lowerCasedItemId.Contains(ArachnoFeast.KeyTerm))
        {
            ArachnoFeast.ItemData = itemData;
        }
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadStoreItem -= LoadItem;
    }
}
