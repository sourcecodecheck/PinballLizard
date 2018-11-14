using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetStore(string storeId, string catalogVersion)
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            CatalogVersion = catalogVersion,
            StoreId = storeId
        },
        (result) => 
        {
            foreach(PlayFab.ClientModels.StoreItem item in result.Store)
            {
                StoreEvents.SendLoadItem(item.ItemId, (int)item.VirtualCurrencyPrices[MayhemKey], (int)item.VirtualCurrencyPrices[BugbucksKey]);
            }
        },
        (error) =>
        {
        });
    }
    private void OnDestroy()
    {
        StoreEvents.OnLoadStore -= GetStore;
    }
}
