using UnityEngine;
using UnityEngine.UI;

public class LoadSettingsButton : MonoBehaviour
{
    public Button OnPress;
    
    void Start()
    {
        OnPress.onClick.AddListener(OnClick);
    }

    
    void Update()
    {

    }
    void OnClick()
    {
        MenuTransitionEvents.SendLoadSettingsScreen();
    }
}
