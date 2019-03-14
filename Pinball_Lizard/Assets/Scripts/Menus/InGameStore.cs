using UnityEngine;

public class InGameStore : Pausable
{
    public string StoreId;
    public string CatalogVersion;
    public string keyTermSpicy;
    public string keyTermBomb;
    public string keyTermFeast;

    public StoreItemDetailWindow SpicyWindow;
    public StoreItemDetailWindow BombWindow;
    public StoreItemDetailWindow FeastWindow;

    private StoreItem spicyItem;
    private StoreItem bombItem;
    private StoreItem feastItem;
    private bool storeLoaded;
    
    new void Start()
    {
        base.Start();
        StoreEvents.OnLoadStoreItem += LoadStoreItem;
        StoreEvents.OnStartInGamePurchase += InGameBuyStart;
        StoreEvents.SendLoadStore(StoreId, CatalogVersion);
        StoreEvents.SendLoadInventory(CatalogVersion);
        storeLoaded = false;
    }
    
    void Update()
    {
    }

    void LoadStoreItem(StoreItemData itemData)
    {
        if (itemData.ItemId.ToLower().Contains(keyTermSpicy))
        {
            spicyItem = new StoreItem();
            spicyItem.KeyTerm = keyTermSpicy;
            spicyItem.ItemData = itemData;
            storeLoaded = true;
        }
        else if (itemData.ItemId.ToLower().Contains(keyTermBomb))
        {
            bombItem = new StoreItem();
            bombItem.KeyTerm = keyTermBomb;
            bombItem.ItemData = itemData;
            storeLoaded = true;
        }
        else if (itemData.ItemId.ToLower().Contains(keyTermFeast))
        {
            feastItem = new StoreItem();
            feastItem.KeyTerm = keyTermFeast;
            feastItem.ItemData = itemData;
            storeLoaded = true;
        }
    }

    void InGameBuyStart(PowerUpButton.PowerUp type)
    {
        if (storeLoaded == true &&  ! isPaused)
        {
            GamePlayEvents.SendPause(false);
            switch (type)
            {
                case PowerUpButton.PowerUp.SPICY:
                    SpicyWindow.Item = spicyItem;
                    SpicyWindow.gameObject.SetActive(true);
                    break;
                case PowerUpButton.PowerUp.NUKE:
                    BombWindow.Item = bombItem;
                    BombWindow.gameObject.SetActive(true);
                    break;
                case PowerUpButton.PowerUp.FEAST:
                    FeastWindow.Item = feastItem;
                    FeastWindow.gameObject.SetActive(true);
                    break;
            }
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        StoreEvents.OnLoadStoreItem -= LoadStoreItem;
        StoreEvents.OnStartInGamePurchase -= InGameBuyStart;
    }
}
