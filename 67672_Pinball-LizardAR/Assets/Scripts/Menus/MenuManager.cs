using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    
    public GameObject MainMenuButtons;
    public GameObject SettingsButton;
    public GameObject PlayerInfoBar;
    public GameObject TitleScreen;
    public GameObject HomeButton;
    public GameObject StoreFront;
    public GameObject PlayerInventoryScreen;
    public Canvas MenuParent;

    private static bool hasMainMenuBeenLoaded = false;
    private List<GameObject> menuObjects;
	// Use this for initialization
	void Start () {
        menuObjects = new List<GameObject>();
        if (hasMainMenuBeenLoaded == false)
        {
            LoadTitle();
        }
        else
        {
            LoadMainMenu();
        }
        MenuTransitionEvents.OnChangeMenu += ChangeMenu;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ChangeMenu(MenuTransitionEvents.Menus menu)
    {
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
            default:
                LoadMainMenu();
                break;
        }
    }

    private void LoadMainMenu()
    {
        hasMainMenuBeenLoaded = true;
        menuObjects.Add(LoadPlayerInfoBar());
        menuObjects.Add(Instantiate(MainMenuButtons, MenuParent.transform));
        menuObjects.Add(Instantiate(SettingsButton, MenuParent.transform));
    }

    private void LoadTitle()
    {
        menuObjects.Add(Instantiate(TitleScreen, MenuParent.transform));
    }

    private void LoadPlayerInfo()
    {
        menuObjects.Add(LoadPlayerInfoBar());
        menuObjects.Add(Instantiate(HomeButton, MenuParent.transform));
        GameObject inventoryScreen = Instantiate(PlayerInventoryScreen, MenuParent.transform);
        Inventory playerInventory = gameObject.GetComponent<Inventory>();
        inventoryScreen.GetComponent<PlayerInventoryScreen>().PlayerInventory = playerInventory;
        menuObjects.Add(inventoryScreen);
    }

    private void LoadStoreFront()
    {
        menuObjects.Add(Instantiate(HomeButton, MenuParent.transform));
        menuObjects.Add(Instantiate(StoreFront, MenuParent.transform));
        menuObjects.Add(LoadPlayerInfoBar());
    }

    private GameObject LoadPlayerInfoBar()
    {
        GameObject playerInfoBarInstance = Instantiate(PlayerInfoBar, MenuParent.transform);
        Inventory playerInventory = gameObject.GetComponent<Inventory>();
        playerInfoBarInstance.GetComponentInChildren<CurrencyCounters>().PlayerInventory = playerInventory;
        playerInfoBarInstance.GetComponentInChildren<PlayerLevelBar>().PlayerInventory = playerInventory;
        playerInfoBarInstance.GetComponentInChildren<Currencies>().PlayerInventory = playerInventory;
        return playerInfoBarInstance;
    }

    private void UnloadMenu()
    {
        foreach(GameObject menuObject in menuObjects)
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
