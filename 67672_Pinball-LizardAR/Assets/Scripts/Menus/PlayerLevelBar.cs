using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelBar : MonoBehaviour
{
    public Slider ExpSlider;
    public Inventory PlayerInventory;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = 
            Mathf.Min(1f,(float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[0]);
        ExpSlider.value = percentage;
    }
}
