using UnityEngine;
using UnityEngine.UI;

public class PowerUpButton : Pausable
{
    public enum PowerUp { SPICY, NUKE, FEAST }
    public enum PowerUpState { EMPTY, ACTIVE, INUSE}
    public PowerUp PowerUpType;
    public string keyTerm;
    public Inventory Inventory;
    public GameObject BuyButton;
    public Text PowerUpCountDisplay;
    public Button UsePowerUp;
    public float FeastLength;

    private PowerUpState state;
    new void Start()
    {
        UsePowerUp.onClick.AddListener(OnClick);
        state = PowerUpState.EMPTY;
        base.Start();
        if (PowerUpType == PowerUp.SPICY)
        {
            GamePlayEvents.OnSpicyEnd += EndSpicy;
        }
        if (PowerUpType == PowerUp.FEAST)
        {
            GamePlayEvents.OnFeastEnd -= EndFeast;
        }
        StoreEvents.OnUpdateInventoryDisplay += UpdateAmounts;
        UpdateAmounts();
    }

    
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
            if (state == PowerUpState.ACTIVE)
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
                    if (BuyButton != null)
                    {
                        BuyButton.SetActive(true);
                    }
                    state = PowerUpState.EMPTY;
                }
                else
                {
                    if (state == PowerUpState.EMPTY)
                    {
                        state = PowerUpState.ACTIVE;
                    }
                    PowerUpCountDisplay.text = Mathf.Min(Inventory.SpicyMeatABallCount, 99).ToString();
                    if (BuyButton != null)
                    {
                        BuyButton.SetActive(false);
                    }
                }
                break;
            case PowerUp.NUKE:
                if (Inventory.DaBombCount <= 0)
                {
                    PowerUpCountDisplay.text = "0";
                    if (BuyButton != null)
                    {
                        BuyButton.SetActive(true);
                    }
                    state = PowerUpState.EMPTY;
                }
                else
                {
                    if (state == PowerUpState.EMPTY)
                    {
                        state = PowerUpState.ACTIVE;
                    }
                    PowerUpCountDisplay.text = Mathf.Min(Inventory.DaBombCount, 99).ToString();
                    if (BuyButton != null)
                    {
                        BuyButton.SetActive(false);
                    }
                }
                break;
            case PowerUp.FEAST:
                if (Inventory.ArachnoFeastCount <= 0)
                {
                    PowerUpCountDisplay.text = "0";
                    if (BuyButton != null)
                    {
                        BuyButton.SetActive(true);
                    }
                    state = PowerUpState.EMPTY;
                }
                else
                {
                    if (state == PowerUpState.EMPTY)
                    {
                        state = PowerUpState.ACTIVE;
                    }
                    PowerUpCountDisplay.text = Mathf.Min(Inventory.ArachnoFeastCount, 99).ToString();
                    if (BuyButton != null)
                    {
                        BuyButton.SetActive(false);
                    }
                }
                break;
        }
    }


    private void ActivateSpicy()
    {
        TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "SpicyButton" }, EventNames.UiAction);
        AudioEvents.SendPlayPowerUp();
        GamePlayEvents.SendSpicyReady();
        GamePlayEvents.SendUsePowerUp(keyTerm);
        state = PowerUpState.INUSE;
    }

    private void EndSpicy()
    {
        state = PowerUpState.ACTIVE;
        UpdateAmounts();
    }

    private void ActivateBomb()
    {
        TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "BombButton" }, EventNames.UiAction);
        GamePlayEvents.SendBombDetonated();
        GamePlayEvents.SendUsePowerUp(keyTerm);
        state = PowerUpState.INUSE;

    }

    private void ActivateFeast()
    {
        TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "FeastButton" }, EventNames.UiAction);
        AudioEvents.SendPlayPowerUp();
        GamePlayEvents.SendFeastStart();
        GamePlayEvents.SendUsePowerUp(keyTerm);
        Invoke("EndFeast", FeastLength);
        state = PowerUpState.INUSE;

    }
    private void EndFeast()
    {
        GamePlayEvents.SendFeastEnd();
        state = PowerUpState.ACTIVE;
        UpdateAmounts();
    }

    private new void OnDestroy()
    {
        if (PowerUpType == PowerUp.SPICY)
        {
            GamePlayEvents.OnSpicyEnd -= EndSpicy;
        }

        if (PowerUpType == PowerUp.FEAST)
        {
            GamePlayEvents.OnFeastEnd -= EndFeast;
        }
        StoreEvents.OnUpdateInventoryDisplay -= UpdateAmounts;
        base.OnDestroy();
    }
}
