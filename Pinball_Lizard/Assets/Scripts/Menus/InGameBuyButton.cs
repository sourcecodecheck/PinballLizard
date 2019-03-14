using UnityEngine;
using UnityEngine.UI;

public class InGameBuyButton : MonoBehaviour
{
    public PowerUpButton.PowerUp PowerUpType;
    public Button AddButton;
    void Start()
    {
        AddButton.onClick.AddListener(SendBuyStart);
    }
    
    void Update()
    {
    }

    void SendBuyStart()
    {
        StoreEvents.SendStartInGamePurchase(PowerUpType);
    }

}
