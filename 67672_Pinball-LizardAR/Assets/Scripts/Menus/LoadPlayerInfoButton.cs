using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerInfoButton : MonoBehaviour
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
        MenuEvents.SendLoadPlayerInfoScreen();
    }
}
