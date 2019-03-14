using PlayFab.ClientModels;

public static class StoreEvents
{
    //Subscribers:
    //Store
    public delegate void LoadStore(string storeId, string catalogVersion);
    public static event LoadStore OnLoadStore;
    public static void SendLoadStore(string storeId, string catalogVersion)
    {
        OnLoadStore?.Invoke(storeId, catalogVersion);
    }

    //Subscribers:
    //StoreFront
    //InGameStore
    public delegate void LoadStoreItem(StoreItemData itemData);
    public static event LoadStoreItem OnLoadStoreItem;
    public static void SendLoadStoreItem(StoreItemData itemData)
    {
        OnLoadStoreItem?.Invoke(itemData);
    }

    //Subscribers:
    public delegate void SelectItem(string itemId);
    public static event SelectItem OnSelectItem;
    public static void SendSelectItem(string itemId)
    {
        OnSelectItem?.Invoke(itemId);
    }

    //Subscribers:
    public delegate void ShowPurchaseButton(bool isShown);
    public static event ShowPurchaseButton OnShowPurchaseButton;
    public static void SendShowPurchaseButton(bool isShown)
    {
        OnShowPurchaseButton?.Invoke(isShown);
    }

    //Subscribers:
    public delegate void StartPurchase(string currency);
    public static event StartPurchase OnStartPurchase;
    public static void SendStartPurchase(string currency)
    {
        OnStartPurchase?.Invoke(currency);
    }

    //Subscribers:
    //Store
    public delegate void PurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer);
    public static event PurchaseItem OnPurchaseItem;
    public static void SendPurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer)
    {
        OnPurchaseItem?.Invoke(itemId, currency, catalogVersion, storeId, price, isContainer);
    }

    //Subscribers:
    //Store
    public delegate void LoadInventory(string catalogVersion);
    public static event LoadInventory OnLoadInventory;
    public static void SendLoadInventory(string catalogVersion)
    {
        OnLoadInventory?.Invoke(catalogVersion);
    }

    //Subscribers:
    //Inventory
    public delegate void LoadInventoryItem(ItemInstance instance);
    public static event LoadInventoryItem OnLoadInventoryItem;
    public static void SendLoadInventoryItem(ItemInstance instance)
    {
        OnLoadInventoryItem?.Invoke(instance);
    }

    //Subscribers:
    //Store
    public delegate void ConsumeItem(ItemInstance instance);
    public static event ConsumeItem OnConsumeItem;
    public static void SendConsumeItem(ItemInstance instance)
    {
        OnConsumeItem?.Invoke(instance);
    }

    //Subscribers:
    //Currencies
    public delegate void LoadCurrencies();
    public static event LoadCurrencies OnLoadCurrencies;
    public static void SendLoadCurrencies()
    {
        OnLoadCurrencies?.Invoke();
    }

    //Subscribers:
    //CurrencyCounters
    public delegate void UpdateCurrencyDisplay();
    public static event UpdateCurrencyDisplay OnUpdateCurrencyDisplay;
    public static void SendUpdateCurrencyDisplay()
    {
        OnUpdateCurrencyDisplay?.Invoke();
    }

    //Subscribers:
    //InGameStore
    public delegate void StartInGamePurchase(PowerUpButton.PowerUp type);
    public static event StartInGamePurchase OnStartInGamePurchase;
    public static void SendStartInGamePurchase(PowerUpButton.PowerUp type)
    {
        OnStartInGamePurchase?.Invoke(type);
    }

    //Subscribers:
    //PlayerInventoryScreen
    //PowerUpButton
    public delegate void UpdateInventoryDisplay();
    public static event UpdateInventoryDisplay OnUpdateInventoryDisplay;
    public static void SendUpdateInventoryDisplay()
    {
        OnUpdateInventoryDisplay?.Invoke();
    }

    //Subscribers:
    //ChallengeMode
    public delegate void SubtractAnimosity(int animosityToSubtract);
    public static event SubtractAnimosity OnSubtractAnimosity;
    public static void SendSubtractAnimosity(int animosityToSubtract)
    {
        OnSubtractAnimosity?.Invoke(animosityToSubtract);
    }

    //Subscribers:
    public delegate void AwardItemOnLoss();
    public static event AwardItemOnLoss OnAwardItemOnLoss;
    public static void SendAwardItemOnLoss()
    {
        OnAwardItemOnLoss?.Invoke();
    }

    //Subscribers:
    //Store
    public delegate void OpenContainer(ItemInstance container, string catalogVersion);
    public static event OpenContainer OnOpenContainer;
    public static void SendOpenContainer(ItemInstance container, string catalogVersion)
    {
        OnOpenContainer?.Invoke(container, catalogVersion);
    }

    //Subscribers:
    //InGameStore
    //StoreFront
    public delegate void ReloadStore();
    public static event ReloadStore OnReloadStore;
    public static void SendReloadStore()
    {
        OnReloadStore?.Invoke();
    }
}
