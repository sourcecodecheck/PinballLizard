using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public string KeyTerm;
    public Button SelectButton;
    public GameObject DetailWindow;
    public GameObject WeekendBanner;
    public StoreItemData ItemData;
    public Inventory PlayerInventory;

    private bool isSelected;
    
    void Awake()
    {
        if (SelectButton != null)
        {
            SelectButton.onClick.AddListener(OnClick);
        }

    }

    void Update()
    {
    }

    public void OnClick()
    {
        DetailWindow.SetActive(true);
    }
    public void SetButtonActive(bool isActive)
    {
        if(WeekendBanner != null)
        {
            WeekendBanner.gameObject.SetActive(!isActive);
        }
        SelectButton.gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
    }
}
