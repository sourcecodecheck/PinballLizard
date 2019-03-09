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
    public Inventory PlayerInventory;
    public ChallengeMode ChallengeMode;

    private MenuEvents.Menus currentMenu;
    private static bool hasMainMenuBeenLoaded = false;
    private List<GameObject> menuObjects;

    void Start()
    {
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
        StoreEvents.OnOpenContainerPopUp += LoadContainerPopUp;
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
            currentMenu = MenuEvents.Menus.MAIN;
            hasMainMenuBeenLoaded = true;
            GameObject mainMenuInstance = Instantiate(MainMenuButtons, MenuParent.transform);
            menuObjects.Add(mainMenuInstance);
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
            currentMenu = MenuEvents.Menus.PLAYERINFO;
            GameObject inventoryScreen = Instantiate(PlayerInventoryScreen, MenuParent.transform);
            inventoryScreen.GetComponent<PlayerInventoryScreen>().PlayerInventory = PlayerInventory;
            menuObjects.Add(inventoryScreen);
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
            currentMenu = MenuEvents.Menus.STORE;
            GameObject storeFront = Instantiate(StoreFront, MenuParent.transform);
            storeFront.GetComponent<StoreFront>().PlayerInventory = PlayerInventory;
            menuObjects.Add(storeFront);
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
            currentMenu = MenuEvents.Menus.SPECTATE;
            menuObjects.Add(Instantiate(SpecatatorMenu, MenuParent.transform));
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);

        }
    }

    private void LoadContainerPopUp()
    {
        try
        {
            menuObjects.Add(Instantiate(ContainerPopUp, MenuParent.transform));
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
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.HasViewedTutorial) == false || PlayerPrefs.GetInt(PlayerPrefsKeys.HasViewedTutorial) != 1)
            {
               
                LoadTutorial();
            }
            else
            {
                currentMenu = MenuEvents.Menus.AR;
                PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 0);
                PlayerPrefs.Save();
                menuObjects.Add(Instantiate(ARMenu, MenuParent.transform));
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
            currentMenu = MenuEvents.Menus.DAILY_CHALLENGE;
            PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 1);
            PlayerPrefs.Save();
            GameObject eventScreen = Instantiate(EventBoard, MenuParent.transform);
            ChallengeMode.GetChallengeSeed();
            menuObjects.Add(eventScreen);
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
            currentMenu = MenuEvents.Menus.TUTORIAL;
            UnloadMenu();
            GameObject tutorial = Instantiate(Tutorial, MenuParent.transform);
            menuObjects.Add(tutorial);
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
        StoreEvents.OnOpenContainerPopUp -= LoadContainerPopUp;
        MenuEvents.OnShowGeneralMessage -= ShowGeneralMessageWindow;
        MenuEvents.OnShowContainerPopUp -= ShowContainerPopup;
    }
}
