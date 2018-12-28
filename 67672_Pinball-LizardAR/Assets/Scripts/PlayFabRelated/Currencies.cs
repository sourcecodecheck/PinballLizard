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
    // Use this for initialization
    void Start()
    {
        StoreEvents.OnLoadCurrencies += UpdateCurrency;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCurrency()
    {
        if (PlayerPrefs.HasKey("sessionticket"))
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                (response) =>
                {
                    foreach (string currencyKey in response.VirtualCurrency.Keys)
                    {
                        //Not a switch because it's not fixed values
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
