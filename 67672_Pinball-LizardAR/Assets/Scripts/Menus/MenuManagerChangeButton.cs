using UnityEngine;
using UnityEngine.UI;

public class MenuManagerChangeButton : MonoBehaviour
{
    public MenuTransitionEvents.Menus MenuToChangeTo;
    public bool IsButton;
    
    void Start()
    {
        if (IsButton)
        {
            GetComponent<Button>().onClick.AddListener(ToMenu);
        }
        else
        {
            GetComponentInChildren<Button>().onClick.AddListener(ToMenu);
        }
    }

    
    void Update()
    {

    }

    private void ToMenu()
    {
        MenuTransitionEvents.SendChangeMenu(MenuToChangeTo);
    }
}
