using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelDisplay : MonoBehaviour
{

    public Inventory PlayerInventory;
    public Text LevelText;
    // Use this for initialization
    void Start()
    {
        LevelText.text = PlayerInventory.PlayerLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
