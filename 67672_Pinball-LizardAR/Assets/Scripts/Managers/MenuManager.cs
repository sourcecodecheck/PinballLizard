using System.Collections.Generic;
using System;
using UnityEngine;
using Microsoft.AppCenter.Unity.Crashes;

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
    public Canvas MenuParent;
    public GameObject GeneralMessageWindow;
    public Inventory PlayerInventory;
    public ChallengeMode ChallengeMode;

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
            hasMainMenuBeenLoaded = true;
            GameObject mainMenuInstance = Instantiate(MainMenuButtons, MenuParent.transform);
            mainMenuInstance.GetComponentInChildren<PlayerLevelDisplay>().PlayerInventory = PlayerInventory;
            mainMenuInstance.GetComponentInChildren<PlayerLevelBarEndGame>().PlayerInventory = PlayerInventory;
            menuObjects.Add(mainMenuInstance);
        }
        catch(Exception menuLoading)
        {
#if UNITY_ANDROID
            Crashes.TrackError(menuLoading);
#endif
        }
    }

    private void LoadTitle()
    {
        try
        {
            menuObjects.Add(Instantiate(TitleScreen, MenuParent.transform));
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            Crashes.TrackError(menuLoading);
#endif
        }
    }

    private void LoadPlayerInfo()
    {
        try
        {
            GameObject inventoryScreen = Instantiate(PlayerInventoryScreen, MenuParent.transform);
            inventoryScreen.GetComponentInChildren<CurrencyCounters>().PlayerInventory = PlayerInventory;
            inventoryScreen.GetComponent<PlayerInventoryScreen>().PlayerInventory = PlayerInventory;
            menuObjects.Add(inventoryScreen);
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            Crashes.TrackError(menuLoading);
#endif
        }
    }

    private void LoadStoreFront()
    {
        try
        {
            GameObject storeFront = Instantiate(StoreFront, MenuParent.transform);
            storeFront.GetComponentInChildren<CurrencyCounters>().PlayerInventory = PlayerInventory;
            storeFront.GetComponent<StoreFront>().PlayerInventory = PlayerInventory;
            menuObjects.Add(storeFront);
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            //Crashes on iOS every single time without fail
            Crashes.TrackError(menuLoading);
#endif
        }
    }

    private void LoadContainerPopUp()
    {
        try
        {
            Instantiate(ContainerPopUp, MenuParent.transform);
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            //Crashes on iOS every single time without fail
            Crashes.TrackError(menuLoading);
#endif
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
                PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 0);
                PlayerPrefs.Save();
                menuObjects.Add(Instantiate(ARMenu, MenuParent.transform));
            }
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            //Crashes on iOS every single time without fail
            Crashes.TrackError(menuLoading);
#endif
        }
    }

    private void LoadDailyChallenge()
    {
        try
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 1);
            PlayerPrefs.Save();
            GameObject eventScreen = Instantiate(EventBoard, MenuParent.transform);
            eventScreen.GetComponentInChildren<CurrencyCounters>().PlayerInventory = PlayerInventory;
            AnimosityCost animosityPopUp = eventScreen.GetComponentInChildren<AnimosityCost>();
            animosityPopUp.PlayerInventory = PlayerInventory;
            animosityPopUp.gameObject.SetActive(false);
            ChallengeMode.GetChallengeSeed();
            menuObjects.Add(eventScreen);
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            //Crashes on iOS every single time without fail
            Crashes.TrackError(menuLoading);
#endif
        }
    }
    private void LoadTutorial()
    {
        try
        {
            UnloadMenu();
            GameObject tutorial = Instantiate(Tutorial, MenuParent.transform);
            menuObjects.Add(tutorial);
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            //Crashes on iOS every single time without fail
            Crashes.TrackError(menuLoading);
#endif
        }
    }
    private void ShowGeneralMessageWindow(string message)
    {
        try
        {
            GameObject messageWindow = Instantiate(GeneralMessageWindow, MenuParent.transform);
            messageWindow.GetComponentInChildren<GeneralMessage>().SetMessage(message);
        }
        catch (Exception menuLoading)
        {
#if UNITY_ANDROID
            //Crashes on iOS every single time without fail
            Crashes.TrackError(menuLoading);
#endif
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
    }
}
