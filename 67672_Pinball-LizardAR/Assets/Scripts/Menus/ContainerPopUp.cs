using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContainerPopUp : MonoBehaviour
{
    public string MayhemKey;
    public string BugBucksKey;
    public string GluttonyKey;
    public string AnimosityKey;
    public string SpicyKeyTerm;
    public string BombKeyTerm;
    public string FeastKeyTerm;

    public Text SpicyText;
    public Text BombText;
    public Text FeastText;
    public Text MayhemText;
    public Text BugBucksText;
    public Text AnimosityText;
    public Text GluttonyText;

    // Use this for initialization
    void Start()
    {
        StoreEvents.OnContainerOpened += ReceiveContainerItems;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReceiveContainerItems(List<ItemInstance> items, Dictionary<string, uint> currencies)
    {
        int bombCount = items.Where((item) => item.ItemId.ToLower().Contains(BombKeyTerm)).Count();
        int spicyCount = items.Where((item) => item.ItemId.ToLower().Contains(SpicyKeyTerm)).Count();
        int feastCount = items.Where((item) => item.ItemId.ToLower().Contains(FeastKeyTerm)).Count();
        SpicyText.text = spicyCount.ToString();
        BombText.text = bombCount.ToString();
        FeastText.text = feastCount.ToString();
        if (currencies.ContainsKey(MayhemKey))
        {
            MayhemText.text = currencies[MayhemKey].ToString();
        }
        if (currencies.ContainsKey(BugBucksKey))
        {
            BugBucksText.text = currencies[BugBucksKey].ToString();
        }
        if (currencies.ContainsKey(AnimosityKey))
        {
            AnimosityText.text = currencies[AnimosityKey].ToString();
        }
        if (currencies.ContainsKey(GluttonyKey))
        {
            GluttonyText.text = currencies[GluttonyKey].ToString();
        }
    }

    private void OnDestroy()
    {
        StoreEvents.OnContainerOpened -= ReceiveContainerItems;
    }
}
