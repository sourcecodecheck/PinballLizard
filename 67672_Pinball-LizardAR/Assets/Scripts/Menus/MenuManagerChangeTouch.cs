using UnityEngine;

public class MenuManagerChangeTouch : MonoBehaviour
{
    public MenuTransitionEvents.Menus MenuToChangeTo;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            MenuTransitionEvents.SendChangeMenu(MenuToChangeTo);
        }
    }
}
