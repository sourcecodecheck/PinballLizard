using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimosityCost : MonoBehaviour
{

    public Button OnPress;
    public Inventory PlayerInventory;
    public GameObject NotEnough;

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

    }

    void AnimosityCheck()
    {
        if (PlayerInventory.AnimosityCount > 0)
        {
            StoreEvents.SendSubtractAnimosity(1);
            MenuTransitionEvents.SendChangeMenu(MenuTransitionEvents.Menus.AR);
        }
        else
        {
            MenuTransitionEvents.SendChangeMenu(MenuTransitionEvents.Menus.MAIN);
        }
    }
}
