using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmBehavior : MonoBehaviour
{

    public bool isRightArm;

    public Sprite ArmPassive;
    public Sprite ArmActive;

    private Vector2 touchStartPosition;
    private bool isActive;
    // Use this for initialization
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    DoDrag();
                    break;
                case TouchPhase.Ended:
                    Vector2 touchTraveled = touch.position - touchStartPosition;
                    DoSwipe(touchTraveled);
                    break;
            }
        }
        if(isActive)
        {
            GetComponent<Image>().sprite = ArmActive;
        }
        else
        {
            GetComponent<Image>().sprite = ArmPassive;
        }
    }
    private void OnCollision(Collision collision)
    {
        if(isActive)
        {
            if(collision.gameObject.name.ToLower().Contains("shot"))
            {
               
            }
        }
    }

    private void DoDrag()
    {
        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit[] draghits = Physics.RaycastAll(ray);
            foreach (RaycastHit draghit in draghits)
            {
                GameObject hitobject = draghit.transform.gameObject;
                if (draghit.distance < 0.5f &&
                    hitobject.name.ToLower().Contains("shot") && hitobject.GetComponent<ShotBehavior>().HasHitBuilding == true)
                {
                    hitobject.transform.rotation = Camera.main.transform.rotation;
                    hitobject.GetComponent<ShotBehavior>().HasHitBuilding = false;

                }
            }
        }
    }

    private void DoSwipe(Vector2 touchTraveled)
    {
            if (touchTraveled.x > 0)
            {
                if (!isRightArm)
                {
                    isActive = true;
                }
            }
            else if (touchTraveled.x < 0)
            {
                if (isRightArm)
                {
                    isActive = true;
                }
            }
            Invoke("ResetArmPosition", 0.5f);
    }

    private void ResetArmPosition()
    {
        isActive = false;
    }
}
