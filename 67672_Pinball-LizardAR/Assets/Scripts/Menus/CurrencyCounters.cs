﻿using UnityEngine;
using UnityEngine.UI;

public class CurrencyCounters : MonoBehaviour
{

    public Inventory PlayerInventory;
    public Text MayhemCount;
    public Text BugBucksCount;
    public Text AnimosityCount;
    public Text GluttonyCount;

    // Use this for initialization
    void Start()
    {
        StoreEvents.OnUpdateCurrencyDisplay += UpdateDisplay;
        StoreEvents.SendLoadCurrencies();
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateDisplay()
    {
        MayhemCount.text = PlayerInventory.MayhemCount.ToString();
        BugBucksCount.text = PlayerInventory.BugBucksCount.ToString();
        AnimosityCount.text = PlayerInventory.AnimosityCount.ToString();
        GluttonyCount.text = PlayerInventory.GluttonyCount.ToString();
    }
    private void OnDestroy()
    {
        StoreEvents.OnUpdateCurrencyDisplay -= UpdateDisplay;
    }
}
