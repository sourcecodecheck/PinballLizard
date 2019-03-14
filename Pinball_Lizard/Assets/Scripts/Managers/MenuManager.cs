using System.Collections.Generic;
using System;
using UnityEngine;
using Microsoft.AppCenter.Unity.Crashes;
using PlayFab.ClientModels;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuButtons;
    public GameObject SettingsButton;
    public GameObject TitleScreen;
    public GameObject StoreFront;
    public GameObject PlayerInventoryScreen;
    public GameObject ARMenu;
    public GameObject EventBoard;
    public GameObject ContainerPopUp;
    public GameObject Tutorial;
    public GameObject SpecatatorMenu;
    public Canvas MenuParent;
    public GameObject GeneralMessageWindow;
    public GameObject HUD;
    public GameObject HomeButton;
    public Inventory PlayerInventory;
    public ChallengeMode ChallengeMode;

    private MenuEvents.Menus currentMenu;
    private static bool hasMainMenuBeenLoaded = false;
    private List<GameObject> menuObjects;
    private DateTime timeofLastMenuChange;

    void Start()
    {
        HomeButton.SetActive(false);
        menuObjects = new List<GameObject>();
        if (hasMainMenuBeenLoaded == false)
        {
           
            LoadTitle();
        }
        else
        {
            PlayerInventory.enabled = true;
            
            LoadMainMenu();
        }
        MenuEvents.OnChangeMenu += ChangeMenu;
        MenuEvents.OnShowGeneralMessage += ShowGeneralMessageWindow;
        MenuEvents.OnShowContainerPopUp += ShowContainerPopup;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ChangeMenu(MenuEvents.Menus menu)
    {
        PlayerInventory.enabled = true;
        TrackingEvents.SendLoadPlayerInfo();
        HUD.SetActive(true);
        
        if (currentMenu == menu)
        {
            return;
        }
        UnloadMenu();
        switch (menu)
        {
            case MenuEvents.Menus.TITLE:
                LoadTitle();
                break;
            case MenuEvents.Menus.MAIN:
                LoadMainMenu();
                break;
            case MenuEvents.Menus.PLAYERINFO:
                LoadPlayerInfo();
                break;
            case MenuEvents.Menus.STORE:
                LoadStoreFront();
                break;
            case MenuEvents.Menus.AR:
                LoadAR();
                break;
            case MenuEvents.Menus.DAILY_CHALLENGE:
                LoadDailyChallenge();
                break;
            case MenuEvents.Menus.SPECTATE:
                LoadSpectator();
                break;
            case MenuEvents.Menus.TUTORIAL:
                LoadSpectator();
                break;
            default:
                LoadMainMenu();
                break;
        }
    }

    private void LoadMainMenu()
    {
        try
        {
            //PlayerPrefs.SetInt(PlayerPrefsKeys.HasViewedTutorial, 0);
            TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
            {
                PreviousScreenID = currentMenu.ToString(),
                PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                CurrentScreenID = MenuEvents.Menus.MAIN.ToString()
            }, EventNames.ScreenChange);
            HomeButton.SetActive(false);
            currentMenu = MenuEvents.Menus.MAIN;
            hasMainMenuBeenLoaded = true;
            GameObject mainMenuInstance = Instantiate(MainMenuButtons, MenuParent.transform);
            menuObjects.Add(mainMenuInstance);
            timeofLastMenuChange = DateTime.Now;
        }
        catch(Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void LoadTitle()
    {
        try
        {
            currentMenu = MenuEvents.Menus.TITLE;
            menuObjects.Add(Instantiate(TitleScreen, MenuParent.transform));
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void LoadPlayerInfo()
    {
        try
        {
            HomeButton.SetActive(true);
            TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
            {
                PreviousScreenID = currentMenu.ToString(),
                PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                CurrentScreenID = MenuEvents.Menus.PLAYERINFO.ToString()
            }, EventNames.ScreenChange);
            currentMenu = MenuEvents.Menus.PLAYERINFO;
            GameObject inventoryScreen = Instantiate(PlayerInventoryScreen, MenuParent.transform);
            inventoryScreen.GetComponent<PlayerInventoryScreen>().PlayerInventory = PlayerInventory;
            menuObjects.Add(inventoryScreen);
            timeofLastMenuChange = DateTime.Now;
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void LoadStoreFront()
    {
        try
        {
            HomeButton.SetActive(true);
            TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
            {
                PreviousScreenID = currentMenu.ToString(),
                PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                CurrentScreenID = MenuEvents.Menus.STORE.ToString()
            }, EventNames.ScreenChange);
            currentMenu = MenuEvents.Menus.STORE;
            GameObject storeFront = Instantiate(StoreFront, MenuParent.transform);
            storeFront.GetComponent<StoreFront>().PlayerInventory = PlayerInventory;
            menuObjects.Add(storeFront);
            timeofLastMenuChange = DateTime.Now;
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }
    private void LoadSpectator()
    {
        try
        {
            HomeButton.SetActive(true);
            TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
            {
                PreviousScreenID = currentMenu.ToString(),
                PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                CurrentScreenID = MenuEvents.Menus.SPECTATE.ToString()
            }, EventNames.ScreenChange);
            currentMenu = MenuEvents.Menus.SPECTATE;
            menuObjects.Add(Instantiate(SpecatatorMenu, MenuParent.transform));
            timeofLastMenuChange = DateTime.Now;
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);

        }
    }

    private void LoadAR()
    {
        try
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.HasViewedTutorial) == false || PlayerPrefs.GetInt(PlayerPrefsKeys.HasViewedTutorial) != 1 
                && currentMenu!= MenuEvents.Menus.DAILY_CHALLENGE)
            {
                LoadTutorial();
            }
            else
            {
                HomeButton.SetActive(true);
                TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
                {
                    PreviousScreenID = currentMenu.ToString(),
                    PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                    CurrentScreenID = MenuEvents.Menus.AR.ToString()
                }, EventNames.ScreenChange);
                if (currentMenu != MenuEvents.Menus.DAILY_CHALLENGE)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 0);
                    PlayerPrefs.Save();
                }
                currentMenu = MenuEvents.Menus.AR;
                menuObjects.Add(Instantiate(ARMenu, MenuParent.transform));
                timeofLastMenuChange = DateTime.Now;
            }
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void LoadDailyChallenge()
    {
        try
        {
            HomeButton.SetActive(true);
            TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
            {
                PreviousScreenID = currentMenu.ToString(),
                PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                CurrentScreenID = MenuEvents.Menus.DAILY_CHALLENGE.ToString()
            }, EventNames.ScreenChange);
            currentMenu = MenuEvents.Menus.DAILY_CHALLENGE;
            PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 1);
            PlayerPrefs.Save();
            GameObject eventScreen = Instantiate(EventBoard, MenuParent.transform);
            ChallengeMode.GetChallengeSeed();
            menuObjects.Add(eventScreen);
            timeofLastMenuChange = DateTime.Now;
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }
    private void LoadTutorial()
    {
        try
        {
            HomeButton.SetActive(false);
            TrackingEvents.SendBuildPlayerEvent(new PlayerScreenChange()
            {
                PreviousScreenID = currentMenu.ToString(),
                PrevScreenDuration = (int)(DateTime.Now - timeofLastMenuChange).TotalSeconds,
                CurrentScreenID = MenuEvents.Menus.TUTORIAL.ToString()
            }, EventNames.ScreenChange);
            currentMenu = MenuEvents.Menus.TUTORIAL;
            UnloadMenu();
            GameObject tutorial = Instantiate(Tutorial, MenuParent.transform);
            menuObjects.Add(tutorial);
            timeofLastMenuChange = DateTime.Now;
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }
    private void ShowGeneralMessageWindow(string message)
    {
        try
        {
            GameObject messageWindow = Instantiate(GeneralMessageWindow, MenuParent.transform);
            messageWindow.GetComponentInChildren<GeneralMessage>().SetMessage(message);
            menuObjects.Add(messageWindow);
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void ShowContainerPopup(List<ItemInstance> items, Dictionary<string, uint> currencies)
    {
        try
        {
            Instantiate(ContainerPopUp, MenuParent.transform).GetComponent<ContainerPopUp>().ReceiveContainerItems(items, currencies);
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void UnloadMenu()
    {
        foreach (GameObject menuObject in menuObjects)
        {
            Destroy(menuObject);
        }
        menuObjects.Clear();
    }
    private void OnDestroy()
    {
        MenuEvents.OnChangeMenu -= ChangeMenu;
        MenuEvents.OnShowGeneralMessage -= ShowGeneralMessageWindow;
        MenuEvents.OnShowContainerPopUp -= ShowContainerPopup;
    }
}
