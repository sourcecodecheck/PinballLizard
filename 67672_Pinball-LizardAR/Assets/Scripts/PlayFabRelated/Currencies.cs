using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Currencies : MonoBehaviour
{

    public string MayhemKey;
    public string AnimosityKey;
    public string BugBucksKey;
    public Inventory PlayerInventory;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateCurrency", 0.5f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateCurrency()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                (response) =>
                {
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
                },
                (error) =>
                {
                    Debug.Log(error.ErrorMessage);
                });
        }
    }
}
