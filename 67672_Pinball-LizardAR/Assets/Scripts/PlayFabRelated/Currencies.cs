using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Microsoft.AppCenter.Unity.Crashes;
using System;


public class Currencies : MonoBehaviour
{
    public string MayhemKey;
    public string AnimosityKey;
    public string BugBucksKey;
    public string GluttonyKey;
    public Inventory PlayerInventory;
    
    void Awake()
    {
        StoreEvents.OnLoadCurrencies += UpdateCurrency;
    }
    
    void Update()
    {
    }

    public void UpdateCurrency()
    {
        //if we've logged in
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //retrieve user's inventory
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                (response) =>
                {
                    //notify and update currency displays and inventory
                    foreach (string currencyKey in response.VirtualCurrency.Keys)
                    {
                        if (currencyKey == MayhemKey)
                        {
                            PlayerInventory.MayhemCount = response.VirtualCurrency[currencyKey];
                        }
                        else if (currencyKey == AnimosityKey)
                        {
                            PlayerInventory.AnimosityCount = response.VirtualCurrency[currencyKey];
                        }
                        else if (currencyKey == BugBucksKey)
                        {
                            PlayerInventory.BugBucksCount = response.VirtualCurrency[currencyKey];
                        }
                    }
                    StoreEvents.SendUpdateCurrencyDisplay();
                },
                (error) =>
                {
                    Debug.Log(error);
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
        StoreEvents.OnLoadCurrencies -= UpdateCurrency;
    }
}
