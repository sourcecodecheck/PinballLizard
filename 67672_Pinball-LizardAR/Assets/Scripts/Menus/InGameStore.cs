using UnityEngine;

public class InGameStore : MonoBehaviour
{
    public string StoreId;
    public string CatalogVersion;
    public string keyTermSpicy;
    public string keyTermBomb;
    public string keyTermFeast;

    public StoreItemDetailWindow SpicyWindow;
    public StoreItemDetailWindow BombWindow;
    public StoreItemDetailWindow FeastWindow;
    
    void Start()
    {
        StoreEvents.OnLoadStoreItem += LoadStoreItem;
        StoreEvents.OnStartInGamePurchase += InGameBuyStart;
        StoreEvents.SendLoadStore(StoreId, CatalogVersion);
        StoreEvents.SendLoadInventory(CatalogVersion);
    }
    
    void Update()
    {
    }

    void LoadStoreItem(StoreItemData itemData)
    {
        if (itemData.ItemId.ToLower().Contains(keyTermSpicy) && SpicyWindow != null)
        {
            StoreItem relatedItem = new StoreItem();
            relatedItem.KeyTerm = keyTermSpicy;
            relatedItem.ItemData = itemData;
            SpicyWindow.Item = relatedItem;
        }
        else if (itemData.ItemId.ToLower().Contains(keyTermBomb) && BombWindow != null)
        {
            StoreItem relatedItem = new StoreItem();
            relatedItem.KeyTerm = keyTermBomb;
            relatedItem.ItemData = itemData;
            BombWindow.Item = relatedItem;
        }
        else if (itemData.ItemId.ToLower().Contains(keyTermFeast) && FeastWindow != null)
        {
            StoreItem relatedItem = new StoreItem();
            relatedItem.KeyTerm = keyTermFeast;
            relatedItem.ItemData = itemData;
            FeastWindow.Item = relatedItem;
        }
    }

    void InGameBuyStart(PowerUpButton.PowerUp type)
    {
        GamePlayEvents.SendPause(false);
        switch (type)
        {
            case PowerUpButton.PowerUp.SPICY:
                SpicyWindow.gameObject.SetActive(true);
                break;
            case PowerUpButton.PowerUp.NUKE:
                BombWindow.gameObject.SetActive(true);
                break;
            case PowerUpButton.PowerUp.FEAST:
                FeastWindow.gameObject.SetActive(true);
                break;
        }
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadStoreItem -= LoadStoreItem;
        StoreEvents.OnStartInGamePurchase -= InGameBuyStart;
    }
}
