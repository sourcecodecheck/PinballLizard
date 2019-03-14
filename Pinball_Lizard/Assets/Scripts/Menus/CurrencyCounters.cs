using UnityEngine;
using UnityEngine.UI;

public class CurrencyCounters : MonoBehaviour
{ 
    public Inventory PlayerInventory;
    public Text MayhemCount;
    public Text BugBucksCount;
    public Text AnimosityCount;
    public Text GluttonyCount;
    
    void Start()
    {
        StoreEvents.OnUpdateCurrencyDisplay += UpdateDisplay;
        StoreEvents.SendLoadCurrencies();
        UpdateDisplay();
    }
    
    void Update()
    {
    }

    private void UpdateDisplay()
    {
        MayhemCount.text = Mathf.Min(PlayerInventory.MayhemCount, 9999).ToString();
        BugBucksCount.text = Mathf.Min(PlayerInventory.BugBucksCount, 9999).ToString();
        AnimosityCount.text = Mathf.Min(PlayerInventory.AnimosityCount, 9999).ToString();
        GluttonyCount.text = Mathf.Min(PlayerInventory.GluttonyCount, 9999).ToString();
    }
    private void OnDestroy()
    {
        StoreEvents.OnUpdateCurrencyDisplay -= UpdateDisplay;
    }
}
