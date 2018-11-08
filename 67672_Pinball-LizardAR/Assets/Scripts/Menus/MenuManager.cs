using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    
    public GameObject MainMenuButtons;
    public GameObject SettingsButton;
    public GameObject PlayerInfoBar;
    public GameObject TitleScreen;
    public GameObject HomeButton;
    public Canvas MenuParent;

    private List<GameObject> menuObjects;
	// Use this for initialization
	void Start () {
        menuObjects = new List<GameObject>();
        LoadTitle();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LoadMainMenu()
    {
        UnloadMenu();
        GameObject playerInfoBarInstance = Instantiate(PlayerInfoBar, MenuParent.transform);
        Inventory playerInventory = gameObject.GetComponent<Inventory>();
        playerInfoBarInstance.GetComponentInChildren<CurrencyCounters>().PlayerInventory = playerInventory;
        playerInfoBarInstance.GetComponentInChildren<Currencies>().PlayerInventory = playerInventory;
        menuObjects.Add(playerInfoBarInstance);
        GameObject mainMenuButtonInstance = Instantiate(MainMenuButtons, MenuParent.transform);
        mainMenuButtonInstance.GetComponentInChildren<MenuManagerChangeButton>().MenuManager = this;
        menuObjects.Add(mainMenuButtonInstance);
        menuObjects.Add(Instantiate(SettingsButton, MenuParent.transform));
    }

    private void LoadTitle()
    {
        UnloadMenu();
        GameObject titleScreenInstance = Instantiate(TitleScreen, MenuParent.transform);
        titleScreenInstance.GetComponent<MenuManagerChangeButton>().MenuManager = this;
        menuObjects.Add(titleScreenInstance);
    }

    private void LoadPlayerInfo()
    {
        UnloadMenu();
        GameObject playerInfoBarInstance = Instantiate(PlayerInfoBar, MenuParent.transform);
        Inventory playerInventory  = gameObject.GetComponent<Inventory>();
        playerInfoBarInstance.GetComponentInChildren<CurrencyCounters>().PlayerInventory = playerInventory;
        playerInfoBarInstance.GetComponentInChildren<Currencies>().PlayerInventory = playerInventory;
        menuObjects.Add(playerInfoBarInstance);
        GameObject homeButtonInstance = Instantiate(HomeButton, MenuParent.transform);
        homeButtonInstance.GetComponentInChildren<MenuManagerChangeButton>().MenuManager = this;
        menuObjects.Add(homeButtonInstance);
    }


    private void UnloadMenu()
    {
        foreach(GameObject menuObject in menuObjects)
        {
            Destroy(menuObject);
        }
        menuObjects.Clear();
    }
}
