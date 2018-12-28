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
    // Use this for initialization
    void Start()
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

    // Update is called once per frame
    void Update()
    {

    }

}
