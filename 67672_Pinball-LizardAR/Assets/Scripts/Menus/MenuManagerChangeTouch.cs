using UnityEngine;

public class MenuManagerChangeTouch : MonoBehaviour
{
    public MenuTransitionEvents.Menus MenuToChangeTo;
    
    void Start()
    {
    }

    
    void Update()
    {
        if(Input.touchCount > 0)
        {
            MenuTransitionEvents.SendChangeMenu(MenuToChangeTo);
        }
    }
}
