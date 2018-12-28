using UnityEngine;
using UnityEngine.UI;

public class LoadPlayerInfoButton : MonoBehaviour
{
    public Button OnPress;
    // Use this for initialization
    void Start()
    {
        OnPress.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        MenuTransitionEvents.SendLoadPlayerInfoScreen();
    }
}
