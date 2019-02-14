using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelBar : MonoBehaviour
{
    public Slider ExpSlider;
    public Inventory PlayerInventory;
    public bool IsStatic;

    private float percentage;
    private int nextLevel;
    
    void Start()
    {
        percentage = 0f;
        nextLevel = 0;
    }
    
    void Update()
    {
        if (PlayerInventory != null && PlayerInventory.PlayerLevel != 0)
        {
            if (IsStatic == false)
            {
                percentage +=
                   (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[nextLevel] * Time.deltaTime;
                float truePercentage = (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[nextLevel];
                percentage = Mathf.Min(percentage, truePercentage);
                if (percentage >= 1)
                {
                    nextLevel = Mathf.Min(nextLevel + 1, PlayerInventory.ExperienceToNextLevel.Count - 1);
                    //play sound effect
                }
                
            }
            else
            {
                percentage = Mathf.Min(1f, (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[nextLevel]);
            }
            ExpSlider.value = percentage;
        }
    }
}
