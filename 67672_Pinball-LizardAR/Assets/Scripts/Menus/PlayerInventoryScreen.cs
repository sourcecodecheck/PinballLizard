using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryScreen : MonoBehaviour
{
    public string CatalogVersion;
    public Inventory PlayerInventory;
    public Text LevelText;
    public Text UserIdText;
    public Text DateJoinedText;
    public Text HighestScoreText;
    public Text BestComboText;
    public Text SpicyCountText;
    public Text BombCountText;
    public Text FeastCountText;
    
    void Start()
    {
        StoreEvents.OnUpdateInventoryDisplay += UpdateInventoryDisplay;
        UpdateInventoryDisplay();
    }

    void Update()
    {
    }

    void UpdateInventoryDisplay()
    {
        LevelText.text = PlayerInventory.PlayerLevel.ToString();
        UserIdText.text = PlayerInventory.PlayerId;
        DateJoinedText.text = PlayerInventory.DateJoined.ToString();
        HighestScoreText.text = PlayerInventory.BestScore.ToString();
        BestComboText.text = PlayerInventory.BestCombo.ToString();
        SpicyCountText.text = PlayerInventory.SpicyMeatABallCount.ToString();
        BombCountText.text = PlayerInventory.DaBombCount.ToString();
        FeastCountText.text = PlayerInventory.ArachnoFeastCount.ToString();
    }

    private void OnDestroy()
    {
        StoreEvents.OnUpdateInventoryDisplay -= UpdateInventoryDisplay;
    }
}
