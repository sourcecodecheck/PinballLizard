using UnityEngine;
using UnityEngine.UI;

public class StoreItemDetailWindow : MonoBehaviour
{
    public Button BuyMayhemButton;
    public Button BuyBugBucksButton;
    public Button BuyGluttonyButton;
    public Button BackButton;
    public Text MayhemPriceText;
    public Text BugBucksPriceText;
    public Text GluttonyPriceText;
    public Text AmountOwnedText;
    public StoreItem Item;
    public bool SendUnpause;
    public bool IsContainer;

    void Awake()
    {
        if (BuyMayhemButton != null)
        {
            BuyMayhemButton.onClick.AddListener(PurchaseMayhem);
        }
        if (BuyBugBucksButton != null)
        {
            BuyBugBucksButton.onClick.AddListener(PurchaseBugBucks);
        }
        if (BuyGluttonyButton != null)
        {
            BuyGluttonyButton.onClick.AddListener(PurchaseGluttony);
        }
        if (BackButton != null)
        {
            BackButton.onClick.AddListener(Back);
        }
        if (MayhemPriceText != null)
        {
            MayhemPriceText.text = Item.ItemData.MayhemPrice.ToString();
        }
        if (BugBucksPriceText != null)
        {
            BugBucksPriceText.text = Item.ItemData.BugBucksPrice.ToString();
        }
        if (GluttonyPriceText != null)
        {
            GluttonyPriceText.text = Item.ItemData.GluttonyPrice.ToString();
        }
        if (AmountOwnedText != null)
        {
            AmountOwnedText.text = "x" + Item.PlayerInventory.GetItemAmount(Item.ItemData.ItemId).ToString();
        }
    }
    void Update()
    {

    }
    public void PurchaseMayhem()
    {
        StoreEvents.SendPurchaseItem(Item.ItemData.ItemId, Item.ItemData.MayhemKey,
            Item.ItemData.CatalogVersion, Item.ItemData.StoreId, Item.ItemData.MayhemPrice, IsContainer);
        Back();
    }


    public void PurchaseBugBucks()
    {
        StoreEvents.SendPurchaseItem(Item.ItemData.ItemId, Item.ItemData.BugBucksKey,
            Item.ItemData.CatalogVersion, Item.ItemData.StoreId, Item.ItemData.BugBucksPrice, IsContainer);
        Back();
    }

    public void PurchaseGluttony()
    {
        StoreEvents.SendPurchaseItem(Item.ItemData.ItemId, Item.ItemData.GluttonyKey, Item.ItemData.CatalogVersion,
            Item.ItemData.StoreId, Item.ItemData.GluttonyPrice, IsContainer);
        Back();
    }

    private void Back()
    {
        gameObject.SetActive(false);
        if (SendUnpause)
        {
            GamePlayEvents.SendPause(false);
        }
    }

    private void OnDestroy()
    {
    }
}
