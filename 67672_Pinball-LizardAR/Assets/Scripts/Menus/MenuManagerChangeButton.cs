using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerChangeButton : MonoBehaviour
{
    public MenuTransitionEvents.Menus MenuToChangeTo;
    public bool IsButton;
    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

    }

    private void ToMenu()
    {
        MenuTransitionEvents.SendChangeMenu(MenuToChangeTo);
    }
}
