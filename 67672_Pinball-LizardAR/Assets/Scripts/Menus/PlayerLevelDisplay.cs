using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelDisplay : MonoBehaviour
{

    public Inventory PlayerInventory;
    public Text LevelText;
    
    void Start()
    {
        LevelText.text = PlayerInventory.PlayerLevel.ToString();
    }

    
    void Update()
    {

    }
}
