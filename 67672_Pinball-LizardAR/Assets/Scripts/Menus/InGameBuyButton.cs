using UnityEngine;
using UnityEngine.UI;

public class InGameBuyButton : MonoBehaviour
{

    // Use this for initialization
    public PowerUpButton.PowerUp PowerUpType;
    public Button AddButton;
    void Start()
    {
        AddButton.onClick.AddListener(SendBuyStart);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SendBuyStart()
    {
        StoreEvents.SendStartInGamePurchase(PowerUpType);
    }

}
