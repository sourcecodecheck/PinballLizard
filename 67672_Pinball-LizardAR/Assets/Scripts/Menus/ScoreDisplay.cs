using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;
    public GameObject Burst;
    public GameObject Animation;

    private int score;
    void Start()
    {
        ScoreEvents.OnScoreUpdated += UpdateScore;
        GamePlayEvents.OnFeastStart += FeastStart;
        GamePlayEvents.OnFeastEnd += FeastEnd;
        AnimationEvents.OnDoublePointsExit += HideAnimation;
        score = 0;
        scoreText.text = score.ToString();
    }
    
    void Update()
    {
    }

    void FeastStart()
    {
        Animation.SetActive(true);
        Burst.SetActive(true);
    }
    void FeastEnd()
    {
        Animation.SetActive(false);
        Burst.SetActive(false);
    }

    void HideAnimation()
    {
        Animation.SetActive(false);
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
