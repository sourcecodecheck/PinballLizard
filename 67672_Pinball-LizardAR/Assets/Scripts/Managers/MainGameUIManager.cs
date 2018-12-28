using UnityEngine;

public class MainGameUIManager : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject PlayerInfoScreen;

    public Canvas MenuParent;
    // Use this for initialization
    void Start()
    {
        GamePlayEvents.OnLoadPauseMenu += LoadPauseMenu;
        MenuTransitionEvents.OnLoadPlayerInfoScreen += LoadPlayerInfoScreen;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadPauseMenu()
    {
        Instantiate(PauseMenu, MenuParent.transform);
    }

    void LoadPlayerInfoScreen()
    {
        PlayerInfoScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnLoadPauseMenu -= LoadPauseMenu;
        MenuTransitionEvents.OnLoadPlayerInfoScreen -= LoadPlayerInfoScreen;
    }
}
