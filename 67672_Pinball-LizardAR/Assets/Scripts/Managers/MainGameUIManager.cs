using UnityEngine;

public class MainGameUIManager : MonoBehaviour
{
    //instantiated
    public GameObject PauseMenu;
    public GameObject ContainerPopUp;
    public GameObject GeneralMessageWindow;
    //show/hide
    public GameObject BuyItemsScreen;
    public GameObject Banner;
    public GameObject Miss;
    //canvas
    public Canvas MenuParent;

    void Start()
    {
        GamePlayEvents.OnLoadPauseMenu += LoadPauseMenu;
        MenuEvents.OnLoadPlayerInfoScreen += LoadPlayerInfoScreen;
        MenuEvents.OnShowGeneralMessage += ShowGeneralMessageWindow;
        AnimationEvents.OnBannerEnter += ShowBanner;
        AnimationEvents.OnBannerExited += HideBanner;
        StoreEvents.OnOpenContainerPopUp += LoadContainerPopUp;
        AnimationEvents.OnMissEnter += ShowMiss;
        AnimationEvents.OnMissExited += HideMiss;
    }

    void Update()
    {

    }

    void LoadPauseMenu()
    {
        Instantiate(PauseMenu, MenuParent.transform);
    }

    void LoadPlayerInfoScreen()
    {
        BuyItemsScreen.SetActive(true);
    }
    void LoadContainerPopUp()
    {
        Instantiate(ContainerPopUp, MenuParent.transform);
    }

    private void HideBanner()
    {
        Banner.SetActive(false);
    }

    private void ShowBanner()
    {
        Banner.SetActive(true);
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
        GameObject messageWindow = Instantiate(GeneralMessageWindow, MenuParent.transform);
        messageWindow.GetComponentInChildren<GeneralMessage>().SetMessage(message);
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnLoadPauseMenu -= LoadPauseMenu;
        MenuEvents.OnLoadPlayerInfoScreen -= LoadPlayerInfoScreen;
        AnimationEvents.OnBannerEnter -= ShowBanner;
        AnimationEvents.OnBannerExited -= HideBanner;
        MenuEvents.OnShowGeneralMessage -= ShowGeneralMessageWindow;
        AnimationEvents.OnMissEnter -= ShowMiss;
        AnimationEvents.OnMissExited -= HideMiss;
    }
}
