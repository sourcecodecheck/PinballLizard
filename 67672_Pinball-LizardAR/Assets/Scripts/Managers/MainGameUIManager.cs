using UnityEngine;
using System;
using Microsoft.AppCenter.Unity.Crashes;
using System.Collections.Generic;
using PlayFab.ClientModels;

public class MainGameUIManager : MonoBehaviour
{
    //instantiated
    public GameObject PauseMenu;
    public GameObject ContainerPopUp;
    public GameObject GeneralMessageWindow;
    //show/hide
    public GameObject BuyItemsScreen;
    public GameObject Miss;
    //canvas
    public Canvas MenuParent;
    public Canvas ContainerCanvas;

    void Start()
    {
        GamePlayEvents.OnLoadPauseMenu += LoadPauseMenu;
        MenuEvents.OnLoadPlayerInfoScreen += LoadPlayerInfoScreen;
        MenuEvents.OnShowGeneralMessage += ShowGeneralMessageWindow;
        MenuEvents.OnSwitchCanvas += SwitchCanvas;
        AnimationEvents.OnMissEnter += ShowMiss;
        AnimationEvents.OnMissExited += HideMiss;
        MenuEvents.OnShowContainerPopUp += ShowContainerPopup;
    }

    void Update()
    {

    }

    void LoadPauseMenu()
    {
        try
        {
            Instantiate(PauseMenu, MenuParent.transform);
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    void LoadPlayerInfoScreen()
    {
        BuyItemsScreen.SetActive(true);
    }

    private void HideMiss()
    {
        Miss.SetActive(false);
    }

    private void ShowMiss()
    {
        Miss.SetActive(true);
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
            Crashes.TrackError(menuLoading);
        }
    }
    private void ShowContainerPopup(List<ItemInstance> items, Dictionary<string, uint> currencies)
    {
        try
        {
            Instantiate(ContainerPopUp, ContainerCanvas.transform).GetComponent<ContainerPopUp>().ReceiveContainerItems(items, currencies);
        }
        catch (Exception menuLoading)
        {
            Crashes.TrackError(menuLoading);
        }
    }

    private void SwitchCanvas(Canvas toSwitchTo)
    {
        MenuParent = toSwitchTo;
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnLoadPauseMenu -= LoadPauseMenu;
        MenuEvents.OnLoadPlayerInfoScreen -= LoadPlayerInfoScreen;
        MenuEvents.OnShowGeneralMessage -= ShowGeneralMessageWindow;
        AnimationEvents.OnMissEnter -= ShowMiss;
        AnimationEvents.OnMissExited -= HideMiss;
        MenuEvents.OnSwitchCanvas -= SwitchCanvas;
        MenuEvents.OnShowContainerPopUp -= ShowContainerPopup;
    }
}
