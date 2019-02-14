using UnityEngine;

public class MenuManagerChangeTouch : MonoBehaviour
{
    public MenuEvents.Menus MenuToChangeTo;
    
    void Start()
    {
    }
    
    void Update()
    {
        if(Input.touchCount > 0)
        {
            MenuEvents.SendChangeMenu(MenuToChangeTo);
        }
    }
}
