using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelDisplay : MonoBehaviour
{
    public Inventory PlayerInventory;
    public Text LevelText;
    
    void Start()
    {
        MenuEvents.OnUpdateLevelDisplay += UpdateDisplay;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (PlayerInventory != null)
        {
            LevelText.text = PlayerInventory.PlayerLevel.ToString();
        }
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        MenuEvents.OnUpdateLevelDisplay -= UpdateDisplay;
    }
}
