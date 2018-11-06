using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour {

    // Use this for initialization
    public Text scoreText;
    private int score;
	void Start () {
        ScoreEvents.OnScoreUpdated += UpdateScore;
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = score.ToString();
    }

   void UpdateScore(int currentScore)
    {
        score = currentScore;
        scoreText.text = score.ToString();
    }
    private void OnDestroy()
    {
        ScoreEvents.OnScoreUpdated -= UpdateScore;
    }
}
