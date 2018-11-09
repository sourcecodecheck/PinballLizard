using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryDisplay : MonoBehaviour
{
    public Text ScoreText;
    public Text BugsText;
    // Use this for initialization
    void Awake()
    {
        TrackingEvents.OnGameVictory += GameVictory;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameVictory(int score, int bugsEaten)
    {
        ScoreText.text += score.ToString();
        BugsText.text += bugsEaten.ToString();
    }

    public void OnDestroy()
    {
        TrackingEvents.OnGameVictory -= GameVictory;
    }
}
