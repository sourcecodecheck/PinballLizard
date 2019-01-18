using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;
    private int score;
    void Start()
    {
        ScoreEvents.OnScoreUpdated += UpdateScore;
        GamePlayEvents.OnFeastStart += FeastStart;
        GamePlayEvents.OnFeastEnd += FeastEnd;
        score = 0;
        scoreText.text = score.ToString();
    }

    
    void Update()
    {

    }

    void FeastStart()
    {
        scoreText.color = new Color(1f, 1f, 0f);
    }
    void FeastEnd()
    {
        scoreText.color = new Color(1f, 1f, 1f);
    }

    void UpdateScore(int currentScore)
    {
        score = currentScore;
        scoreText.text = score.ToString();
    }
    private void OnDestroy()
    {
        GamePlayEvents.OnFeastStart -= FeastStart;
        GamePlayEvents.OnFeastEnd -= FeastEnd;
        ScoreEvents.OnScoreUpdated -= UpdateScore;
    }
}
