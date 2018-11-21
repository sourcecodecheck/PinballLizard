using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class StoreEvents
{
    public delegate void LoadStore(string storeId, string catalogVersion);
    public static event LoadStore OnLoadStore;
    public static void SendLoadStore(string storeId, string catalogVersion)
    {
        OnLoadStore(storeId, catalogVersion);
    }

    public delegate void LoadStoreItem(string itemId, string itemName, int mayhemPrice, int bugBucksPrice, string storeId, string bugBucksKey, string mayhemKey);
    public static event LoadStoreItem OnLoadStoreItem;
    public static void SendLoadStoreItem(string itemId, string itemName, int mayhemPrice, int bugBucksPrice, string storeId, string bugBucksKey, string mayhemKey)
    {
        OnLoadStoreItem(itemId, itemName, mayhemPrice, bugBucksPrice, storeId, bugBucksKey, mayhemKey);
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

    public delegate void LoadInventoryItem(string itemId, string itemName, int count);
    public static event LoadInventoryItem OnLoadInventoryItem;
    public static void SendLoadInventoryItem(string itemId, string itemName, int count)
    {
        OnLoadInventoryItem(itemId, itemName, count);
    }
}
