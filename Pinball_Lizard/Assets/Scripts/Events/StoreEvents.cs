using PlayFab.ClientModels;
using System.Collections.Generic;

public static class StoreEvents
{
    public delegate void LoadStore(string storeId, string catalogVersion);
    public static event LoadStore OnLoadStore;
    public static void SendLoadStore(string storeId, string catalogVersion)
    {
        OnLoadStore?.Invoke(storeId, catalogVersion);
    }

    public delegate void LoadStoreItem(StoreItemData itemData);
    public static event LoadStoreItem OnLoadStoreItem;
    public static void SendLoadStoreItem(StoreItemData itemData)
    {
        OnLoadStoreItem?.Invoke(itemData);
    }
    
    public delegate void SelectItem(string itemId);
    public static event SelectItem OnSelectItem;
    public static void SendSelectItem(string itemId)
    {
        OnSelectItem?.Invoke(itemId);
    }

    public delegate void ShowPurchaseButton(bool isShown);
    public static event ShowPurchaseButton OnShowPurchaseButton;
    public static void SendShowPurchaseButton(bool isShown)
    {
        OnShowPurchaseButton?.Invoke(isShown);
    }

    public delegate void StartPurchase(string currency);
    public static event StartPurchase OnStartPurchase;
    public static void SendStartPurchase(string currency)
    {
        OnStartPurchase?.Invoke(currency);
    }

    public delegate void PurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer);
    public static event PurchaseItem OnPurchaseItem;
    public static void SendPurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer)
    {
        OnPurchaseItem?.Invoke(itemId, currency, catalogVersion, storeId, price, isContainer);
    }

    public delegate void LoadInventory(string catalogVersion);
    public static event LoadInventory OnLoadInventory;
    public static void SendLoadInventory(string catalogVersion)
    {
        OnLoadInventory?.Invoke(catalogVersion);
    }

    public delegate void LoadInventoryItem(ItemInstance instance);
    public static event LoadInventoryItem OnLoadInventoryItem;
    public static void SendLoadInventoryItem(ItemInstance instance)
    {
        OnLoadInventoryItem?.Invoke(instance);
    }

    public delegate void ContainerOpened(List<ItemInstance> items, Dictionary<string, uint> currencies);
    public static event ContainerOpened OnContainerOpened;
    public static void SendContainerOpened(List<ItemInstance> items, Dictionary<string, uint> currencies)
    {
        OnContainerOpened?.Invoke(items, currencies);
    }

    public delegate void ConsumeItem(ItemInstance instance);
    public static event ConsumeItem OnConsumeItem;
    public static void SendConsumeItem(ItemInstance instance)
    {
        OnConsumeItem?.Invoke(instance);
    }

    public delegate void OpenContainerPopUp();
    public static event OpenContainerPopUp OnOpenContainerPopUp;
    public static void SendOpenContainerPopUp()
    {
        OnOpenContainerPopUp?.Invoke();
    }

    public delegate void LoadCurrencies();
    public static event LoadCurrencies OnLoadCurrencies;
    public static void SendLoadCurrencies()
    {
        OnLoadCurrencies?.Invoke();
    }

    public delegate void UpdateCurrencyDisplay();
    public static event UpdateCurrencyDisplay OnUpdateCurrencyDisplay;
    public static void SendUpdateCurrencyDisplay()
    {
        OnUpdateCurrencyDisplay?.Invoke();
    }

    public delegate void StartInGamePurchase(PowerUpButton.PowerUp type);
    public static event StartInGamePurchase OnStartInGamePurchase;
    public static void SendStartInGamePurchase(PowerUpButton.PowerUp type)
    {
        OnStartInGamePurchase?.Invoke(type);
    }

    public delegate void UpdateInventoryDisplay();
    public static event UpdateInventoryDisplay OnUpdateInventoryDisplay;
    public static void SendUpdateInventoryDisplay()
    {
        OnUpdateInventoryDisplay?.Invoke();
    }

    public delegate void SubtractAnimosity(int animosityToSubtract);
    public static event SubtractAnimosity OnSubtractAnimosity;
    public static void SendSubtractAnimosity(int animosityToSubtract)
    {
        OnSubtractAnimosity?.Invoke(animosityToSubtract);
    }

    public delegate void AwardItemOnLoss();
    public static event AwardItemOnLoss OnAwardItemOnLoss;
    public static void SendAwardItemOnLoss()
    {
        OnAwardItemOnLoss?.Invoke();
    }
}
