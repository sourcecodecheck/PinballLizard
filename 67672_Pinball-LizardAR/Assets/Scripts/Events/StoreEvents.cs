using PlayFab.ClientModels;

public static class StoreEvents
{
    public delegate void LoadStore(string storeId, string catalogVersion);
    public static event LoadStore OnLoadStore;
    public static void SendLoadStore(string storeId, string catalogVersion)
    {
        OnLoadStore(storeId, catalogVersion);
    }

    public delegate void LoadStoreItem(StoreItemData itemData);
    public static event LoadStoreItem OnLoadStoreItem;
    public static void SendLoadStoreItem(StoreItemData itemData)
    {
        OnLoadStoreItem(itemData);
    }
    
    public delegate void SelectItem(string itemId);
    public static event SelectItem OnSelectItem;
    public static void SendSelectItem(string itemId)
    {
        OnSelectItem(itemId);
    }

    public delegate void ShowPurchaseButton(bool isShown);
    public static event ShowPurchaseButton OnShowPurchaseButton;
    public static void SendShowPurchaseButton(bool isShown)
    {
        OnShowPurchaseButton(isShown);
    }

    public delegate void StartPurchase(string currency);
    public static event StartPurchase OnStartPurchase;
    public static void SendStartPurchase(string currency)
    {
        OnStartPurchase(currency);
    }

    public delegate void PurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price);
    public static event PurchaseItem OnPurchaseItem;
    public static void SendPurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price)
    {
        OnPurchaseItem(itemId, currency, catalogVersion, storeId, price);
    }

    public delegate void LoadInventory(string catalogVersion);
    public static event LoadInventory OnLoadInventory;
    public static void SendLoadInventory(string catalogVersion)
    {
        OnLoadInventory(catalogVersion);
    }

    public delegate void LoadInventoryItem(ItemInstance instance);
    public static event LoadInventoryItem OnLoadInventoryItem;
    public static void SendLoadInventoryItem(ItemInstance instance)
    {
        OnLoadInventoryItem(instance);
    }

    public delegate void ConsumeItem(ItemInstance instance);
    public static event ConsumeItem OnConsumeItem;
    public static void SendConsumeItem(ItemInstance instance)
    {
        OnConsumeItem(instance);
    }

    public delegate void LoadCurrencies();
    public static event LoadCurrencies OnLoadCurrencies;
    public static void SendLoadCurrencies()
    {
        OnLoadCurrencies();
    }

    public delegate void UpdateCurrencyDisplay();
    public static event UpdateCurrencyDisplay OnUpdateCurrencyDisplay;
    public static void SendUpdateCurrencyDisplay()
    {
        OnUpdateCurrencyDisplay();
    }

    public delegate void StartInGamePurchase(PowerUpButton.PowerUp type);
    public static event StartInGamePurchase OnStartInGamePurchase;
    public static void SendStartInGamePurchase(PowerUpButton.PowerUp type)
    {
        OnStartInGamePurchase(type);
    }

    public delegate void UpdateInventoryDisplay();
    public static event UpdateInventoryDisplay OnUpdateInventoryDisplay;
    public static void SendUpdateInventoryDisplay()
    {
        OnUpdateInventoryDisplay();
    }
}
