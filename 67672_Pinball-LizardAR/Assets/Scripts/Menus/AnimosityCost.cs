using UnityEngine;
using UnityEngine.UI;

public class AnimosityCost : MonoBehaviour
{
    public Button OnPress;
    public Inventory PlayerInventory;
    public GameObject NotEnough;

    void Start()
    {
        OnPress.onClick.AddListener(AnimosityCheck);
        if (PlayerInventory.AnimosityCount > 0)
        {
            NotEnough.SetActive(false);
        }
        else
        {
            NotEnough.SetActive(true);
        }
    }

    void Update()
    {

    }

    void AnimosityCheck()
    {
        if (PlayerInventory.AnimosityCount > 0)
        {
            StoreEvents.SendSubtractAnimosity(1);
            MenuEvents.SendChangeMenu(MenuEvents.Menus.AR);
        }
        else
        {
            MenuEvents.SendChangeMenu(MenuEvents.Menus.MAIN);
        }
    }
}
