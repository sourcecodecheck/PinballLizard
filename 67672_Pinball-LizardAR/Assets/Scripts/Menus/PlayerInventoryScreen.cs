using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryScreen : MonoBehaviour
{
    public string CatalogVersion;
    public Inventory PlayerInventory;

    public Text UserIdText;
    public Text DateJoinedText;
    public Text HighestScoreText;
    public Text BestComboText;

    void Start()
    {
        StoreEvents.OnUpdateInventoryDisplay += UpdateInventoryDisplay;
        TrackingEvents.SendLoadPlayerInfo();
        UpdateInventoryDisplay();
    }

    void Update()
    {
    }

    void UpdateInventoryDisplay()
    {
        UserIdText.text = PlayerPrefs.GetString(PlayerPrefsKeys.PlayFabId);
        DateJoinedText.text = PlayerInventory.DateJoined.ToString();
        HighestScoreText.text = PlayerInventory.BestScore.ToString();
        BestComboText.text = PlayerInventory.BestCombo.ToString();
    }

    private void OnDestroy()
    {
        StoreEvents.OnUpdateInventoryDisplay -= UpdateInventoryDisplay;
    }
}
