using System.Linq;
using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class Store : MonoBehaviour
{
    public string MayhemKey;
    public string BugBucksKey;
    public string GluttonyKey;
    

    void Awake()
    {
        StoreEvents.OnLoadStore += GetStore;
        StoreEvents.OnPurchaseItem += PurchaseItem;
        StoreEvents.OnLoadInventory += GetUserInventory;
        StoreEvents.OnConsumeItem += ConsumeItem;
    }

    
    void Update()
    {

    }

    public void PurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
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
                if(isContainer == true)
                {
                    StoreEvents.SendOpenContainerPopUp();
                    ConsumeContainer(result.Items.First(), catalogVersion);
                }
                else
                {
                    StoreEvents.SendLoadCurrencies();
                    StoreEvents.SendLoadInventory(catalogVersion);
                }
            },
            (error) =>
            {
                ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
            });
        }
    }
    public void GetStore(string storeId, string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            if (ItemCatalog.isLoaded != true)
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
                GetIsEventTime();
                foreach (PlayFab.ClientModels.StoreItem item in result.Store)
                {
                    int mayhemPrice = -1;
                    int bugBucksPrice = -1;
                    int gluttonyPrice = -1; 
                    if(item.VirtualCurrencyPrices.ContainsKey(MayhemKey))
                    {
                        mayhemPrice = (int)item.VirtualCurrencyPrices[MayhemKey];
                    }
                    if (item.VirtualCurrencyPrices.ContainsKey(BugBucksKey))
                    {
                        bugBucksPrice = (int)item.VirtualCurrencyPrices[BugBucksKey];
                    }
                    if (item.VirtualCurrencyPrices.ContainsKey(GluttonyKey))
                    {
                        gluttonyPrice = (int)item.VirtualCurrencyPrices[GluttonyKey];
                    }
                    StoreEvents.SendLoadStoreItem(new StoreItemData {
                        ItemId = item.ItemId,
                        CatalogVersion = catalogVersion,
                        MayhemPrice = mayhemPrice,
                        BugBucksPrice = bugBucksPrice,
                        GluttonyPrice = gluttonyPrice,
                        MayhemKey = MayhemKey,
                        BugBucksKey = BugBucksKey,
                        GluttonyKey = GluttonyKey,
                        StoreId = result.StoreId
                    });
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
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            if(ItemCatalog.isLoaded != true)
            {
                GetCatalog(catalogVersion);
            }
            PlayFabClientAPI.GetUserInventory(
                new GetUserInventoryRequest(),
                (result) => 
                {
                    foreach( ItemInstance item in result.Inventory )
                    {
                        StoreEvents.SendLoadInventoryItem(item);
                    }
                },
                (error) => 
                {
                    ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
                });
        }
    }

    private void ConsumeItem(ItemInstance itemInstance)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
            {
                ConsumeCount = 1,
                ItemInstanceId = itemInstance.ItemInstanceId
            },
            (result) => 
            {
            },
            (error) =>
            {
            });
        }
    }

    private void ConsumeContainer(ItemInstance itemInstance, string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            PlayFabClientAPI.UnlockContainerInstance(new UnlockContainerInstanceRequest
            {
                ContainerItemInstanceId = itemInstance.ItemInstanceId
            },
            (result) =>
            {
                StoreEvents.SendContainerOpened(result.GrantedItems, result.VirtualCurrency);
                StoreEvents.SendLoadCurrencies();
                StoreEvents.SendLoadInventory(catalogVersion);
            },
            (error) =>
            {
            });
        }
    }

    private void GetIsEventTime()
    {
        PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "initializePlayer",
                   FunctionParameter = new { timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours }
               },
               (result) =>
               {
                   bool isEvent = (bool)((JsonObject)result.FunctionResult)["isEventTime"];
                   PlayerPrefs.SetInt(PlayerPrefsKeys.EventSet, isEvent ? 1 : 0);
                   PlayerPrefs.Save();
               },
               (error) =>
               {
               });
    }

    public void GetCatalog(string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            PlayFabClientAPI.GetCatalogItems(
            new GetCatalogItemsRequest()
            {
                CatalogVersion = catalogVersion
            },
            (result) =>
            {
                ItemCatalog.LoadItems(result.Catalog);
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
        StoreEvents.OnPurchaseItem -= PurchaseItem;
        StoreEvents.OnLoadInventory -= GetUserInventory;
        StoreEvents.OnConsumeItem -= ConsumeItem;
    }
}
