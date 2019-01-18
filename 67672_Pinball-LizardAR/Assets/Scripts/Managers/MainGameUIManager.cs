using UnityEngine;

public class MainGameUIManager : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject PlayerInfoScreen;
    public GameObject SettingsScreen;

    public Canvas MenuParent;
    
    void Start()
    {
        GamePlayEvents.OnLoadPauseMenu += LoadPauseMenu;
        MenuTransitionEvents.OnLoadPlayerInfoScreen += LoadPlayerInfoScreen;
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
        SettingsScreen.SetActive(true);
    }

    void LoadSettingsScreen()
    {
        PlayerInfoScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnLoadPauseMenu -= LoadPauseMenu;
        MenuTransitionEvents.OnLoadPlayerInfoScreen -= LoadPlayerInfoScreen;
    }
}
