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
            GamePlayEvents.SendPause(true);
            if (LoadPauseMenu)
            {
                GamePlayEvents.SendLoadPauseMenu();
            }
        }
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
    }
}
