using UnityEngine;

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
            AnimationEvents.OnRightHandSwipe += SwipeAnimation;
            AnimationEvents.OnRightHandSwiped += HandleArmSwiped;
        }
        else
        {
            AnimationEvents.OnLeftHandSwipe += SwipeAnimation;
            AnimationEvents.OnLeftHandSwiped += HandleArmSwiped;
        }
    }


    void Update()
    { 
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

    private void SwipeAnimation()
    {
        if (isActive)
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
