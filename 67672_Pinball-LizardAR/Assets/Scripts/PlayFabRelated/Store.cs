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
    void Start()
    {
        StoreEvents.OnLoadStore += GetStore;
        StoreEvents.OnPurchaseItem += StartPurchase;
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
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
            {
                CatalogVersion = catalogVersion,
                StoreId = storeId
            },
            (result) =>
            {
                foreach (PlayFab.ClientModels.StoreItem item in result.Store)
                {
                    StoreEvents.SendLoadItem(item.ItemId,
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
    private void OnDestroy()
    {
        StoreEvents.OnLoadStore -= GetStore;
        StoreEvents.OnPurchaseItem -= StartPurchase;
    }
}
