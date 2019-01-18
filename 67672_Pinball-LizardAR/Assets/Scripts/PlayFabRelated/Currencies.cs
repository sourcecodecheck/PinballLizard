using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Currencies : MonoBehaviour
{
    public string MayhemKey;
    public string AnimosityKey;
    public string BugBucksKey;
    public string GluttonyKey;
    public Inventory PlayerInventory;
    
    void Start()
    {
        StoreEvents.OnLoadCurrencies += UpdateCurrency;
    }

    
    void Update()
    {

    }

    public void UpdateCurrency()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
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
                    StoreEvents.SendUpdateCurrencyDisplay();
                },
                (error) =>
                {
                    ShowMessageWindowHelper.ShowMessage(error.ErrorMessage);
                });
        }
    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadCurrencies -= UpdateCurrency;
    }
}
