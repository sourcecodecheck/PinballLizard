using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour
{ 
    public bool IsCancelButton;
    public string VirtualCurrency;
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        if(IsCancelButton)
        {
            StoreEvents.SendShowPurchaseButton(false);
        }
        else
        {
            StoreEvents.SendStartPurchase(VirtualCurrency);
        }
    }
}
