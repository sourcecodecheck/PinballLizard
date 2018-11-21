using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Store : MonoBehaviour
{
    public string MayhemKey;
    public string BugbucksKey;
    // Use this for initialization

    private ItemCatalog itemCatalog;
    void Start()
    {
        itemCatalog = new ItemCatalog();
        StoreEvents.OnLoadStore += GetStore;
        StoreEvents.OnPurchaseItem += StartPurchase;
        StoreEvents.OnLoadInventory += GetUserInventory;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void StartPurchase(string itemId, string currency, string catalogVersion, string storeId, int price)
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
            {
                ItemId = itemId,
                VirtualCurrency = currency,
                CatalogVersion = catalogVersion,
                StoreId = storeId,
                Price = price
            },
            (result) =>
            {
                ShowMessageWindowHelper.ShowMessage(result.Items.First().DisplayName + " Purchased!");
            },
            (error) =>
            {
                ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
            });
        }
    }
    public void GetStore(string storeId, string catalogVersion)
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            if (itemCatalog.isLoaded != true)
            {
                GetCatalog(catalogVersion);
            }
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
            {
                CatalogVersion = catalogVersion,
                StoreId = storeId
            },
            (result) =>
            {
                foreach (PlayFab.ClientModels.StoreItem item in result.Store)
                {
                    StoreEvents.SendLoadStoreItem(item.ItemId, itemCatalog.GetCatalogItem(item.ItemId).DisplayName,
                        (int)item.VirtualCurrencyPrices[MayhemKey], (int)item.VirtualCurrencyPrices[BugbucksKey],
                        result.StoreId, BugbucksKey, MayhemKey);
                }
            },
            (error) =>
            {
                ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
            });
        }
    }

    public void GetUserInventory(string catalogVersion)
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            if(itemCatalog.isLoaded != true)
            {
                GetCatalog(catalogVersion);
            }
            PlayFabClientAPI.GetUserInventory(
                new GetUserInventoryRequest(),
                (result) => 
                {
                    foreach( ItemInstance item in result.Inventory )
                    {
                        StoreEvents.SendLoadInventoryItem(item.ItemId, itemCatalog.GetCatalogItem(item.ItemId).DisplayName, (int)item.RemainingUses);
                    }
                },
                (error) => 
                {
                    ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
                });
        }
    }

    public void GetCatalog(string catalogVersion)
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.GetCatalogItems(
            new GetCatalogItemsRequest()
            {
                CatalogVersion = catalogVersion
            },
            (result) =>
            {
                itemCatalog.LoadItems(result.Catalog);
            },
            (error) =>
            {
                ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
            });
        }
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadStore -= GetStore;
        StoreEvents.OnPurchaseItem -= StartPurchase;
        StoreEvents.OnLoadInventory -= GetUserInventory;
    }
}
