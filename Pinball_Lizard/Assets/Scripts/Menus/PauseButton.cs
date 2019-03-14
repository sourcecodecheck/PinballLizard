using UnityEngine.UI;

public class PauseButton : Pausable
{
    public bool LoadPauseMenu;
    
    new void Start()
    {
        base.Start();
        GetComponent<Button>().onClick.AddListener(ButtonPressed);
    }
    
    void Update()
    {
    }

    void ButtonPressed()
    {
        if (!isPaused)
        {
            TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "PauseButton" }, EventNames.UiAction);
            GamePlayEvents.SendPause(true);
            if (LoadPauseMenu)
            {
                TrackingEvents.SendBuildCityEvent( new CitySessionPause() { }, EventNames.SessionPause);
                GamePlayEvents.SendLoadPauseMenu();
            }
        }
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
    }
}
