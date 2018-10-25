using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelBar : MonoBehaviour
{
    public Inventory PlayerInventory;
    public Image BarBackground;
    public Image BarForeground;
    public Text LevelText;

    private float defaultBarX;
    // Use this for initialization
    void Start()
    {
        defaultBarX = BarForeground.rectTransform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float lerpResult = Mathf.Lerp(0, BarBackground.rectTransform.sizeDelta.x,
            (float)PlayerInventory.ExperienceCount / (float)PlayerInventory.ExperienceToNextLevel);
        BarForeground.rectTransform.sizeDelta = new Vector2(lerpResult,
            BarForeground.rectTransform.sizeDelta.y);
        BarForeground.rectTransform.position = new Vector2(defaultBarX + lerpResult * 0.5f, BarForeground.rectTransform.position.y);
        LevelText.text = "Player Level " + PlayerInventory.PlayerLevel;
    }
}
