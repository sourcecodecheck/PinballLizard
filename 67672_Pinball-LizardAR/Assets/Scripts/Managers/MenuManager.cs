using System;
using System.Collections.Generic;
using UnityEngine;

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
    public Canvas MenuParent;
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
        MenuTransitionEvents.OnChangeMenu += ChangeMenu;
        StoreEvents.OnOpenContainerPopUp += LoadContainerPopUp;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ChangeMenu(MenuTransitionEvents.Menus menu)
    {
        PlayerInventory.enabled = true;
        UnloadMenu();
        switch (menu)
        {
            case MenuTransitionEvents.Menus.TITLE:
                LoadTitle();
                break;
            case MenuTransitionEvents.Menus.MAIN:
                LoadMainMenu();
                break;
            case MenuTransitionEvents.Menus.PLAYERINFO:
                LoadPlayerInfo();
                break;
            case MenuTransitionEvents.Menus.STORE:
                LoadStoreFront();
                break;
            case MenuTransitionEvents.Menus.AR:
                LoadAR();
                break;
            case MenuTransitionEvents.Menus.DAILY_CHALLENGE:
                LoadDailyChallenge();
                break;
            default:
                LoadMainMenu();
                break;
        }
    }

    private void LoadMainMenu()
    {
        hasMainMenuBeenLoaded = true;
        GameObject mainMenuInstance = Instantiate(MainMenuButtons, MenuParent.transform);
        mainMenuInstance.GetComponentInChildren<PlayerLevelBar>().PlayerInventory = PlayerInventory;
        menuObjects.Add(mainMenuInstance);
    }

    private void LoadTitle()
    {
        menuObjects.Add(Instantiate(TitleScreen, MenuParent.transform));
    }

    private void LoadPlayerInfo()
    {
        GameObject inventoryScreen = Instantiate(PlayerInventoryScreen, MenuParent.transform);
        inventoryScreen.GetComponentInChildren<CurrencyCounters>().PlayerInventory = PlayerInventory;
        inventoryScreen.GetComponent<PlayerInventoryScreen>().PlayerInventory = PlayerInventory;
        menuObjects.Add(inventoryScreen);
    }

    private void LoadStoreFront()
    {
        GameObject storeFront = Instantiate(StoreFront, MenuParent.transform);
        storeFront.GetComponentInChildren<CurrencyCounters>().PlayerInventory = PlayerInventory;
        storeFront.GetComponent<StoreFront>().PlayerInventory = PlayerInventory;
        menuObjects.Add(storeFront);
    }

    private void LoadContainerPopUp()
    {
        Instantiate(ContainerPopUp, MenuParent.transform);
    }

    private void LoadAR()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.ChallengeModeSet, 0);
        PlayerPrefs.Save();
        menuObjects.Add(Instantiate(ARMenu, MenuParent.transform));

    }

    private void LoadDailyChallenge()
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
        MenuTransitionEvents.OnChangeMenu -= ChangeMenu;
    }
}
