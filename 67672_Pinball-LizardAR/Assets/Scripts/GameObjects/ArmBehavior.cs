using UnityEngine;
using UnityEngine.UI;

public class ArmBehavior : Pausable
{
    public bool IsRightArm;
    public GameObject ArmEnter;
    public GameObject ArmIdle;
    public GameObject ArmAction;
    public GameObject ArmExit;
    public GameObject Flash;

    private Vector2 touchStartPosition;
    private GameObject activeAnimation;
    private bool isActive;

    // Use this for initialization
    new void Start()
    {
        isActive = false;
        base.Start();
        AnimationEvents.OnHandsEnter += HandleArmEnter;
        AnimationEvents.OnHandsEntered += HandleArmEntered;
        AnimationEvents.OnHandsExit += HandleArmExit;
        AnimationEvents.OnHandsExited += HandleArmExited;
        if (IsRightArm)
        {
            AnimationEvents.OnRightHandSwiped += HandleArmSwiped;
        }
        else
        {
            AnimationEvents.OnLeftHandSwiped += HandleArmSwiped;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && isActive)
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
                ShotBehavior shotBehavior = hitobject.GetComponent<ShotBehavior>();
                if (draghit.distance < (shotBehavior.DeathDistance * 0.2f) && hitobject.name.ToLower().Contains("shot") &&
                    shotBehavior.HasHitBuilding == true)
                {
                    hitobject.transform.rotation = Camera.main.transform.rotation;
                    shotBehavior.HasHitBuilding = false;
                    ScoreEvents.SendAddMultiplier(0.5f);
                }
            }
        }
    }
    private void HandleArmEnter()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        isActive = true;
        activeAnimation = ArmEnter;
        activeAnimation.SetActive(true);
    }
    private void HandleArmEntered()
    {
        if (isActive)
        {
            if (activeAnimation != null)
            {
                activeAnimation.SetActive(false);
            }
            activeAnimation = ArmIdle;
            activeAnimation.SetActive(true);
        }
    }
    private void HandleArmSwiped()
    {
        if (isActive)
        {
            if (activeAnimation != null)
            {
                activeAnimation.SetActive(false);
            }
            activeAnimation = ArmIdle;
            activeAnimation.SetActive(true);
            Flash.SetActive(false);
        }
    }

    private void HandleArmExit()
    {
        if (isActive)
        {
            isActive = false;
            if (activeAnimation != null)
            {
                activeAnimation.SetActive(false);
            }
            activeAnimation = ArmExit;
            activeAnimation.SetActive(true);
        }
    }

    private void HandleArmExited()
    {
        if (!isActive)
        {
            if (activeAnimation != null)
            {
                activeAnimation.SetActive(false);
            }
            activeAnimation = null;
        }
    }

    private void SwipeAnimation(Vector2 touchTraveled)
    {
        if (isActive)
        {
            if (touchTraveled.x > 0 && !IsRightArm)
            {
                if (activeAnimation != null)
                {
                    activeAnimation.SetActive(false);
                }
                if (activeAnimation != ArmAction)
                {
                    activeAnimation = ArmAction;
                    activeAnimation.SetActive(true);
                    Flash.SetActive(true);
                }
            }
            else if (touchTraveled.x < 0 && IsRightArm)
            {
                if (activeAnimation != null)
                {
                    activeAnimation.SetActive(false);
                }
                if (activeAnimation != ArmAction)
                {
                    activeAnimation = ArmAction;
                    activeAnimation.SetActive(true);
                    Flash.SetActive(true);
                }
            }
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        AnimationEvents.OnHandsEnter -= HandleArmEnter;
        AnimationEvents.OnHandsEntered -= HandleArmEntered;
        AnimationEvents.OnHandsExit -= HandleArmExit;
        AnimationEvents.OnHandsExited -= HandleArmExited;
        if (IsRightArm)
        {
            AnimationEvents.OnRightHandSwiped -= HandleArmSwiped;
        }
        else
        {
            AnimationEvents.OnLeftHandSwiped -= HandleArmSwiped;
        }
    }
}
