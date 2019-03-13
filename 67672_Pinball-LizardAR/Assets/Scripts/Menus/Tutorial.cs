using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public RectTransform TutorialImage;
    public float MoveSpeed;

    private int currentStep;
    private float currentPosition;
    private bool isTransitioning;
    public float[] StopPoints;
    // Use this for initialization
    void Start()
    {
        currentStep = 0;
        isTransitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnTouch();
        if (isTransitioning)
        {
            TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "TapTutorial" });
            TutorialImage.transform.position = Vector2.MoveTowards(TutorialImage.transform.position,
                new Vector2(StopPoints[currentStep], TutorialImage.transform.position.y), MoveSpeed);
            if (Mathf.Abs(TutorialImage.transform.position.x - StopPoints[currentStep]) <= 0.1f)
            {
                isTransitioning = false;
                ++currentStep;
            }
        }
    }

    void OnTouch()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && isTransitioning == false)
        {
            if (currentStep >= StopPoints.Length)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.HasViewedTutorial, 1);
                MenuEvents.SendChangeMenu(MenuEvents.Menus.AR);
            }
            else
            {
                isTransitioning = true;
            }
        }
    }
}
