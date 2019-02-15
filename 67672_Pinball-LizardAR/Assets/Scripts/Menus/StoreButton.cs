using UnityEngine;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour
{ 
    public bool IsCancelButton;
    public string VirtualCurrency;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    
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
