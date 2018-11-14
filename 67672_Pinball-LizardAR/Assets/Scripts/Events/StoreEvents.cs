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

    public delegate void LoadItem(string itemId, int mayhemPrice, int bugBucksPrice);
    public static event LoadItem OnLoadItem;
    public static void SendLoadItem(string itemId, int mayhemPrice, int bugBucksPrice)
    {
        OnLoadItem(itemId, mayhemPrice, bugBucksPrice);
    }

    public delegate void SelectItem(string itemId);
    public static event SelectItem OnSelectItem;
    public static void SendSelectItem(string itemId)
    {
        OnSelectItem(itemId);
    }

    public delegate void PurchaseItem(string itemId, string currency);
    public static event PurchaseItem OnPurchaseItem;
    public static void SendPurchaseItem(string itemId, string currency)
    {
        OnPurchaseItem(itemId, currency);
    }
}
