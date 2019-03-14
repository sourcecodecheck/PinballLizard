using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class MenuEvents
{
    public enum Menus {TITLE = 0, MAIN, PLAYERINFO, STORE, SETTINGS, AR, DAILY_CHALLENGE, SPECTATE, TUTORIAL }
    //Subscribers:
    //MenuManager
    public delegate void ChangeMenu(Menus menu);
    public static event ChangeMenu OnChangeMenu;
    public static void SendChangeMenu(Menus menu)
    {
        OnChangeMenu?.Invoke(menu);
    }

    //Subscribers:
    //MainGameUIManager
    public delegate void LoadPlayerInfoScreen();
    public static event LoadPlayerInfoScreen OnLoadPlayerInfoScreen;
    public static void SendLoadPlayerInfoScreen()
    {
        OnLoadPlayerInfoScreen?.Invoke();
    }

    //Subscribers:
    public delegate void LoadSettingsScreen();
    public static event LoadSettingsScreen OnLoadSettingsScreen;
    public static void SendLoadSettingsScreen()
    {
        OnLoadSettingsScreen?.Invoke();
    }

    //Subscribers:
    //PlayerLevelDisplay
    public delegate void UpdateLevelDisplay();
    public static event UpdateLevelDisplay OnUpdateLevelDisplay;
    public static void SendUpdateLevelDisplay()
    {
        OnUpdateLevelDisplay?.Invoke();
    }

    //Subscribers:
    //MainGameUIManager
    //MenuManager
    public delegate void ShowGeneralMessage(string messageToShow);
    public static event ShowGeneralMessage OnShowGeneralMessage;
    public static void SendShowGeneralMessage(string messageToShow)
    {
        OnShowGeneralMessage?.Invoke(messageToShow);
    }

    //Subscribers:
    //MainGameUIManager
    public delegate void SwitchCanvas(Canvas toSwitchTo);
    public static event SwitchCanvas OnSwitchCanvas;
    public static void SendSwitchCanvas(Canvas toSwitchTo)
    {
        OnSwitchCanvas?.Invoke(toSwitchTo);
    }

    //Subscribers:
    //MainGameUIManager
    //MenuManager
    public delegate void ShowContainerPopUp(List<ItemInstance> items, Dictionary<string, uint> currencies);
    public static event ShowContainerPopUp OnShowContainerPopUp;
    public static void SendShowContainerPopUp(List<ItemInstance> items, Dictionary<string, uint> currencies)
    {
        OnShowContainerPopUp?.Invoke(items, currencies);
    }

    //Subscribers:
    //EventTimer
    public delegate void ChallengeModeEndRetrieved(DateTime endTime);
    public static event ChallengeModeEndRetrieved OnChallengeModeEndRetrieved;
    public static void SendChallengeModeEndRetrieved(DateTime endTime)
    {
        OnChallengeModeEndRetrieved?.Invoke(endTime);
    }
}
