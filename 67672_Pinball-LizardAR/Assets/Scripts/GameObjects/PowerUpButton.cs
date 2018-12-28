using UnityEngine;
using UnityEngine.UI;

public class PowerUpButton : Pausable
{
    public enum PowerUp { SPICY, NUKE, FEAST }
    public PowerUp PowerUpType;
    public string keyTerm;
    public Inventory Inventory;
    public GameObject BuyButton;
    public Text PowerUpCountDisplay;
    public Button UsePowerUp;

    private bool isDisabled;
    new void Start()
    {
        UsePowerUp.onClick.AddListener(OnClick);
        isDisabled = true;
        base.Start();
        if (PowerUpType == PowerUp.SPICY)
        {
            GamePlayEvents.OnSpicyEnd += EndSpicy;
        }
        StoreEvents.OnUpdateInventoryDisplay += UpdateAmounts;
        UpdateAmounts();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {

        }
    }

    public void OnClick()
    {
        if (!isPaused)
        {
            if (isDisabled == false)
            {
                switch (PowerUpType)
                {
                    case PowerUp.SPICY:
                        ActivateSpicy();
                        break;
                    case PowerUp.NUKE:
                        ActivateBomb();
                        break;
                    case PowerUp.FEAST:
                        ActivateFeast();
                        break;
                }
                UpdateAmounts();
            }
        }
    }



    private void UpdateAmounts()
    {
        switch (PowerUpType)
        {
            case PowerUp.SPICY:
                if (Inventory.SpicyMeatABallCount <= 0)
                {
                    PowerUpCountDisplay.text = "0";
                    BuyButton.SetActive(true);
                    isDisabled = true;
                }
                else
                {
                    isDisabled = false;
                    PowerUpCountDisplay.text = Inventory.SpicyMeatABallCount.ToString();
                    BuyButton.SetActive(false);
                }
                break;
            case PowerUp.NUKE:
                if (Inventory.DaBombCount <= 0)
                {
                    PowerUpCountDisplay.text = "0";
                    BuyButton.SetActive(true);
                    isDisabled = true;
                }
                else
                {
                    isDisabled = false;
                    PowerUpCountDisplay.text = Inventory.DaBombCount.ToString();
                    BuyButton.SetActive(false);
                }
                break;
            case PowerUp.FEAST:
                if (Inventory.ArachnoFeastCount <= 0)
                {
                    PowerUpCountDisplay.text = "0";
                    BuyButton.SetActive(true);
                    isDisabled = true;
                }
                else
                {
                    isDisabled = false;
                    PowerUpCountDisplay.text = Inventory.ArachnoFeastCount.ToString();
                    BuyButton.SetActive(false);
                }
                break;
        }
    }

    private void ActivateSpicy()
    {
        GamePlayEvents.SendSpicyReady();
        GamePlayEvents.SendUsePowerUp(keyTerm);
        isDisabled = true;
    }

    private void EndSpicy()
    {
        isDisabled = false;
    }

    private void ActivateBomb()
    {
        GamePlayEvents.SendBombDetonated();
        GamePlayEvents.SendUsePowerUp(keyTerm);
        isDisabled = true;
    }

    private void ActivateFeast()
    {
        GamePlayEvents.SendFeastStart();
        GamePlayEvents.SendUsePowerUp(keyTerm);
        Invoke("EndFeast", 10f);
    }
    private void EndFeast()
    {
        GamePlayEvents.SendFeastEnd();
        isDisabled = false;
    }

    private new void OnDestroy()
    {
        if (PowerUpType == PowerUp.SPICY)
        {
            GamePlayEvents.OnSpicyEnd -= EndSpicy;
        }
        StoreEvents.OnUpdateInventoryDisplay -= UpdateAmounts;
        base.OnDestroy();
    }
}
