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
    public List<int> ExperienceToNextLevel;
    public Dictionary<string, ItemInstance> ServerSideItems;
    
    void Start()
    {
        ExperienceToNextLevel = new List<int> { 1000 };
        ServerSideItems = new Dictionary<string, ItemInstance>();
        StoreEvents.OnLoadInventoryItem += LoadServerSideItem;
        StoreEvents.SendLoadInventory(CatalogVersion);
        GamePlayEvents.OnUsePowerUp += UseItem;
        DaBombCount = 0;
        SpicyMeatABallCount = 0;
        ArachnoFeastCount = 0;
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayFabId))
        {
            PlayerId = PlayerPrefs.GetString(PlayerPrefsKeys.PlayFabId);
        }
        TrackingEvents.SendLoadPlayerInfo();
        StoreEvents.SendLoadCurrencies();
    }

    private void LoadServerSideItem(ItemInstance itemInstance)
    {
        if (ServerSideItems.Keys.Contains(itemInstance.ItemInstanceId) == false)
        {
            ServerSideItems.Add(itemInstance.ItemInstanceId, itemInstance);
        }
        else
        {
            ServerSideItems[itemInstance.ItemId] = itemInstance;
        }
        SpicyMeatABallCount = ServerSideItems.Where((item) => item.Key.ToLower().Contains(SpicyKeyTerm))?.Sum((item)=> item.Value.RemainingUses) ?? 1;
        DaBombCount = ServerSideItems.Where((item) => item.Key.ToLower().Contains(BombKeyTerm))?.Sum((item) => item.Value.RemainingUses) ?? 1;
        ArachnoFeastCount = ServerSideItems.Where((item) => item.Key.ToLower().Contains(FeastKeyTerm))?.Sum((item) => item.Value.RemainingUses) ?? 1;
      
        StoreEvents.SendUpdateInventoryDisplay();
    }

    public void UseItem(string keyTerm)
    {
        ItemInstance itemInstance = ServerSideItems.FirstOrDefault((item) => item.Value.ItemId.ToLower().Contains(keyTerm)).Value;
        StoreEvents.SendConsumeItem(itemInstance);
        --itemInstance.RemainingUses;
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
        StoreEvents.SendUpdateInventoryDisplay();
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
    public PlayerBase GetPlayerInfo()
    {
        return new PlayerBase()
        {
            BombCount = DaBombCount,
            FeastCount = ArachnoFeastCount,
            PlayerExperience = ExperienceCount,
            PlayerLevel = PlayerLevel,
            SpicyCount = SpicyMeatABallCount
        };
    }
    
    void Update()
    {

    }

    private void OnDestroy()
    {
        StoreEvents.OnLoadInventoryItem -= LoadServerSideItem;
        GamePlayEvents.OnUsePowerUp -= UseItem;
    }
}
