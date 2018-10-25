using UnityEngine;
using UnityEngine.UI;

public class ArmBehavior : MonoBehaviour
{
    public bool IsRightArm;
    public Sprite ArmPassive;
    public Sprite ArmActive;

    private Vector2 touchStartPosition;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    Volley();
                    break;
                case TouchPhase.Ended:
                    Vector2 touchTraveled = touch.position - touchStartPosition;
                    SwipeAnimation(touchTraveled);
                    Volley();
                    break;
            }
        }
    }

    private void Volley()
    {
        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit[] draghits = Physics.RaycastAll(ray);
            foreach (RaycastHit draghit in draghits)
            {
                GameObject hitobject = draghit.transform.gameObject;
                if (draghit.distance < 0.3f && hitobject.name.ToLower().Contains("shot") &&
                    hitobject.GetComponent<ShotBehavior>().HasHitBuilding == true)
                {
                    hitobject.transform.rotation = Camera.main.transform.rotation;
                    hitobject.GetComponent<ShotBehavior>().HasHitBuilding = false;
                }
            }
        }
    }

    private void SwipeAnimation(Vector2 touchTraveled)
    {
        if (touchTraveled.x > 0 && !IsRightArm)
        {
            GetComponent<Image>().sprite = ArmActive;
        }
        else if (touchTraveled.x < 0 && IsRightArm)
        {
            GetComponent<Image>().sprite = ArmActive;
        }
        Invoke("ResetArmPosition", 0.5f);
    }

    private void ResetArmPosition()
    {
        GetComponent<Image>().sprite = ArmPassive;
    }
}
