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
            if (PlayerInventory.PlayerLevel > 1)
            {
                nextLevel = Mathf.Max(1, nextLevel);
            }
            else
            {
                nextLevel = Mathf.Max(0, nextLevel);
            }
            if (IsStatic == false)
            {
                if (nextLevel > 0)
                {
                    float levelDiff = PlayerInventory.ExperienceToNextLevel[nextLevel] - PlayerInventory.ExperienceToNextLevel[nextLevel - 1];
                    float expAsDiff = PlayerInventory.ExperienceCount - PlayerInventory.ExperienceToNextLevel[nextLevel - 1]; ;
                    percentage +=
                       expAsDiff / levelDiff * Time.deltaTime;
                    float truePercentage = expAsDiff / levelDiff;
                    percentage = Mathf.Min(percentage, truePercentage);
                }
                else
                {
                    percentage +=
                       (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[nextLevel] * Time.deltaTime;
                    float truePercentage = (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[nextLevel];
                    percentage = Mathf.Min(percentage, truePercentage);

                }
                if (percentage >= 1)
                {
                    nextLevel = Mathf.Min(nextLevel + 1, PlayerInventory.ExperienceToNextLevel.Count - 1);
                    percentage = 0f;
                    //play sound effect
                }

            }
            else
            {
                while (percentage == 0f || percentage > 1f)
                {
                    if (PlayerInventory.PlayerLevel > 1 && nextLevel > 0)
                    {
                        float levelDiff = PlayerInventory.ExperienceToNextLevel[nextLevel] - PlayerInventory.ExperienceToNextLevel[nextLevel - 1];
                        float expAsDiff = PlayerInventory.ExperienceCount - PlayerInventory.ExperienceToNextLevel[nextLevel - 1];
                        percentage = expAsDiff / levelDiff;
                    }
                    else
                    {
                        percentage = (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel[nextLevel];
                    }
                    if (percentage > 1f)
                    {
                        ++nextLevel;
                        percentage = 0f;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            ExpSlider.value = percentage;
        }
    }
}
