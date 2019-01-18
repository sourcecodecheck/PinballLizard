using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using System;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public int DaBombCount;
    public int SpicyMeatABallCount;
    public int ArachnoFeastCount;
    public int MayhemCount;
    public int BugBucksCount;
    public int AnimosityCount;
    public int GluttonyCount;
    public int PlayerLevel;
    public int ExperienceCount;
    public int PreviousExperienceCount;
    public List<int> ExperienceToNextLevel;
    public int BugsEatenCount;
    public int LastGameScore;
    public int BestScore;
    public float BestCombo;
    public string CatalogVersion;
    public string SpicyKeyTerm;
    public string BombKeyTerm;
    public string FeastKeyTerm;
    public string PlayerId;
    public DateTime DateJoined;
    public List<ItemInstance> ServerSideItems;

    
    void Start()
    {
        ExperienceToNextLevel = new List<int> { 1000 };
        ServerSideItems = new List<ItemInstance>();
        StoreEvents.OnLoadInventoryItem += LoadServerSideItem;
        StoreEvents.SendLoadInventory(CatalogVersion);
        StoreEvents.OnPurchaseItem += ClearItems;
        GamePlayEvents.OnUsePowerUp += UseItem;
        DaBombCount = 0;
        SpicyMeatABallCount = 0;
        ArachnoFeastCount = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayFabId))
        {
            PlayerId = PlayerPrefs.GetString(PlayerPrefsKeys.PlayFabId);
        }
        TrackingEvents.SendLoadPlayerInfo();
    }

    private void ClearItems(string itemId, string currency, string catalogVersion, string storeId, int price, bool isContainer)
    {
        ServerSideItems.Clear();
        DaBombCount = 0;
        SpicyMeatABallCount = 0;
        ArachnoFeastCount = 0;
    }

    private void LoadServerSideItem(ItemInstance itemInstance)
    {
        ServerSideItems.Add(itemInstance);
        string lowercaseId = itemInstance.ItemId.ToLower();
        if (lowercaseId.Contains(SpicyKeyTerm))
        {
            SpicyMeatABallCount += itemInstance.RemainingUses ?? 1;
        }
        else if (lowercaseId.Contains(BombKeyTerm))
        {
            DaBombCount += itemInstance.RemainingUses ?? 1;
        }
        else if (lowercaseId.Contains(FeastKeyTerm))
        {
            ArachnoFeastCount += itemInstance.RemainingUses ?? 1;
        }
        StoreEvents.SendUpdateInventoryDisplay();
    }

    public void UseItem(string keyTerm)
    {
        StoreEvents.SendConsumeItem(ServerSideItems.FirstOrDefault((item) => item.ItemId.ToLower().Contains(keyTerm)));
        if (keyTerm.Contains(SpicyKeyTerm))
        {
            --SpicyMeatABallCount;
        }
        else if (keyTerm.Contains(BombKeyTerm))
        {
            --DaBombCount;
        }
        else if (keyTerm.Contains(FeastKeyTerm))
        {
            --ArachnoFeastCount;
        }

    }

    public int GetItemAmount(string itemId)
    {
        string lowercaseId = itemId;
        if (lowercaseId.Contains(SpicyKeyTerm))
        {
            return SpicyMeatABallCount;
        }
        else if (lowercaseId.Contains(BombKeyTerm))
        {
            return DaBombCount;
        }
        else if (lowercaseId.Contains(FeastKeyTerm))
        {
            return ArachnoFeastCount;
        }
        return -1;
    }


    
    void Update()
    {

    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadInventoryItem -= LoadServerSideItem;
        StoreEvents.OnPurchaseItem -= ClearItems;
        GamePlayEvents.OnUsePowerUp -= UseItem;
    }
}
