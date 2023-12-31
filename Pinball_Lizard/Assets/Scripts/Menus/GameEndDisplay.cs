﻿using UnityEngine;
using UnityEngine.UI;

public class GameEndDisplay : MonoBehaviour
{
    public Text ScoreText;
    public Text BugsText;
    public Text BestComboText;
    public Text PlayerLevelText;
    public Text PowerUpsUsedText;
    public Inventory PlayerInventory;

    void Awake()
    {
        TrackingEvents.OnGameVictory += GameEnd;
        TrackingEvents.OnGameDefeat += GameEnd;
    }

    void Update()
    {
    }

    public void GameEnd(int score, int bugsEaten, float maxMultiplier, bool isWin)
    {
        ScoreText.text = score.ToString();
        BestComboText.text = Mathf.RoundToInt(maxMultiplier * 10f).ToString();
        PlayerLevelText.text = PlayerInventory.PlayerLevel.ToString();
        BugsText.text = bugsEaten.ToString();
        PowerUpsUsedText.text = Mathf.FloorToInt(score * TitleDataStore.MayhemMultiplier).ToString();
    }

    public void OnDestroy()
    {
        TrackingEvents.OnGameVictory -= GameEnd;
        TrackingEvents.OnGameDefeat -= GameEnd;
    }
}
