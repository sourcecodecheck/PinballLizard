using System.Linq;
using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using Microsoft.AppCenter.Unity.Crashes;


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
        StoreEvents.OnOpenContainer += ConsumeContainer;
    }

    void Update()
    {
    }

    public void PurchaseItem(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer)
    {
        //if we've logged in
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //request to purchase item
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
                //notify and update inventory and displays

                //if it's a container
                if (isContainer == true)
                {
                    //open container
                    //StoreEvents.SendOpenContainerPopUp();
                    ConsumeContainer(result.Items.First(), catalogVersion);
                }
                else
                {
                    //notify and update inventory and displays
                    foreach (ItemInstance item in result.Items)
                    {
                        StoreEvents.SendLoadInventoryItem(item);
                    }
                    StoreEvents.SendLoadCurrencies();
                    StoreEvents.SendLoadInventory(catalogVersion);
                    MenuEvents.SendShowGeneralMessage(result.Items.First().DisplayName + " Purchased!");
                    AudioEvents.SendPlayItemGet();
                    GetStore(storeId, catalogVersion);
                }
            },
            (error) =>
            {
                AudioEvents.SendPlayDown();
                MenuEvents.SendShowGeneralMessage(error.ErrorMessage);
                if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                    error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                {
                    LoginHelper.Login();
                }
                try
                {
                    throw new Exception(error.ErrorMessage);
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }
            });
        }
    }
    public void GetStore(string storeId, string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //if we haven't loaded the catalog yet
            if (ItemCatalog.isLoaded != true)
            {
                //load the catalog
                GetCatalog(catalogVersion);
            }
            //load the specific store
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
            {
                CatalogVersion = catalogVersion,
                StoreId = storeId
            },
            (result) =>
            {
                //check for event time
                GetIsEventTime();
                //notify and update storefront with item details
                foreach (PlayFab.ClientModels.StoreItem item in result.Store)
                {
                    int mayhemPrice = -1;
                    int bugBucksPrice = -1;
                    int gluttonyPrice = -1;
                    if (item.VirtualCurrencyPrices.ContainsKey(MayhemKey))
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
                    StoreEvents.SendLoadStoreItem(new StoreItemData
                    {
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
                Debug.Log(error);
                if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                       error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                {
                    LoginHelper.Login();
                }
                try
                {
                    throw new Exception(error.ErrorMessage);
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }
            });
        }
    }

    public void GetUserInventory(string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //catalog check and load
            if (ItemCatalog.isLoaded != true)
            {
                GetCatalog(catalogVersion);
            }
            //load inventory
            PlayFabClientAPI.GetUserInventory(
                new GetUserInventoryRequest(),
                (result) =>
                {
                    //notify and update inventory of every item
                    foreach (ItemInstance item in result.Inventory)
                    {
                        StoreEvents.SendLoadInventoryItem(item);
                    }
                },
                (error) =>
                {
                    ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
                    if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                        error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                    {
                        LoginHelper.Login();
                    }
                    try
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                    catch (Exception exception)
                    {
                        Crashes.TrackError(exception);
                    }
                });
        }
    }

    private void ConsumeItem(ItemInstance itemInstance)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //request to consume item instance
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
                Debug.Log(error);
                if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                    error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                {
                    LoginHelper.Login();
                }
                try
                {
                    throw new Exception(error.ErrorMessage);
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }
            });
        }
    }

    private void ConsumeContainer(ItemInstance itemInstance, string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //request to open and use container
            PlayFabClientAPI.UnlockContainerInstance(new UnlockContainerInstanceRequest
            {
                ContainerItemInstanceId = itemInstance.ItemInstanceId
            },
            (result) =>
            {
                //notify and update inventory and container display window
                //StoreEvents.SendContainerOpened(result.GrantedItems, result.VirtualCurrency);
                MenuEvents.SendShowContainerPopUp(result.GrantedItems, result.VirtualCurrency);
                foreach (ItemInstance item in result.GrantedItems)
                {
                    StoreEvents.SendLoadInventoryItem(item);
                }
                StoreEvents.SendLoadCurrencies();
                StoreEvents.SendLoadInventory(catalogVersion);
            },
            (error) =>
            {
                Debug.Log(error);
                if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                    error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                {
                    LoginHelper.Login();
                }
                try
                {
                    throw new Exception(error.ErrorMessage);
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }
            });
        }
    }

    private void GetIsEventTime()
    {
        //run cloudscript to check if it's event time
        PlayFabClientAPI.ExecuteCloudScript(
               new ExecuteCloudScriptRequest()
               {
                   FunctionName = "isEventTime",
                   FunctionParameter = new { timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours }
               },
               (result) =>
               {
                   bool isEvent = PlayFabSimpleJson.DeserializeObject<bool>(PlayFabSimpleJson.SerializeObject(((JsonObject)result.FunctionResult)["isEventTime"]));
                   PlayerPrefs.SetInt(PlayerPrefsKeys.EventSet, isEvent ? 1 : 0);
                   PlayerPrefs.Save();
               },
               (error) =>
               {
                   Debug.Log(error);
                   if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                        error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                   {
                       LoginHelper.Login();
                   }
                   try
                   {
                       throw new Exception(error.ErrorMessage);
                   }
                   catch (Exception exception)
                   {
                       Crashes.TrackError(exception);
                   }
               });
    }

    public void GetCatalog(string catalogVersion)
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //retrieve all items in catalog and store in catalog static class
            PlayFabClientAPI.GetCatalogItems(
            new GetCatalogItemsRequest()
            {
                CatalogVersion = catalogVersion
            },
            (result) =>
            {
                GetUserInventory(catalogVersion);
                ItemCatalog.LoadItems(result.Catalog);
            },
            (error) =>
            {
                Debug.Log(error);
                if (error.Error == PlayFabErrorCode.InvalidAuthToken ||
                    error.Error == PlayFabErrorCode.ExpiredAuthToken || error.Error == PlayFabErrorCode.AuthTokenExpired)
                {
                    LoginHelper.Login();
                }
                try
                {
                    throw new Exception(error.ErrorMessage);
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }
            });
        }
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadStore -= GetStore;
        StoreEvents.OnPurchaseItem -= PurchaseItem;
        StoreEvents.OnLoadInventory -= GetUserInventory;
        StoreEvents.OnConsumeItem -= ConsumeItem;
        StoreEvents.OnOpenContainer -= ConsumeContainer;
    }
}
