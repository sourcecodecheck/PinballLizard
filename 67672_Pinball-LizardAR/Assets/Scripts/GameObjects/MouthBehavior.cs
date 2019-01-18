using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouthBehavior : Pausable
{
    public GameObject MouthIntro;
    public GameObject MouthIdle;
    public GameObject MouthOutro;
    public GameObject MouthNom;
    public GameObject IceAmmo;
    public GameObject Reticle;
    public GameObject SpicyShot;
    public Button ReticleButton;

    private enum AmmoTypes { ICE = 0 }
    private enum MouthState { OPEN, NOM, SHOOT, HANDS }
    private Queue<AmmoTypes> ammoQueue;
    private int shotsExisting;
    private MouthState state;
    private bool isSpicyReady;
    private GameObject activeAnimation;
    private bool nomQueued;
    private bool leftHandSwipe;
    private bool isSwiping;

    void Awake()
    {
        leftHandSwipe = true;
        nomQueued = false;
        ammoQueue = new Queue<AmmoTypes>();
        state = MouthState.OPEN;
        isSpicyReady = false;
        AnimationEvents.OnMouthEnter += HandleMouthEnter;
        AnimationEvents.OnMouthEntered += HandleMouthEntered;
        AnimationEvents.OnMouthExited += HandleMouthExited;
        AnimationEvents.OnMouthNommed += HandleMouthNommed;
        GamePlayEvents.OnShotDestroyed += HandleShotDestroyed;
        GamePlayEvents.OnSpicyReady += SpicyReady;
        GamePlayEvents.OnConfirmNom += DoNom;
        GamePlayEvents.OnConfirmVolley += VolleyConfirm;
        ReticleButton.onClick.AddListener(Shoot);
        shotsExisting = 0;
        base.Start();
    }

    void Update()
    {
        OnTouch();
        UpdateMouthState();
    }

    private void OnTouch()
    {
        if (!isPaused)
        {
            if (Input.touchCount > 0)
            {
                if (state == MouthState.OPEN)
                {
                    OpenMouthBehavior();
                }
                else if (state == MouthState.SHOOT)
                {
                    ShootBehavior();
                }
            }
        }
    }

    private void OpenMouthBehavior()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                GamePlayEvents.SendTryNom();
                break;
            }
        }
    }
    private void DoNom()
    {
        ammoQueue.Enqueue(AmmoTypes.ICE);
        TrackingEvents.SendBugEaten();
        if (nomQueued == false)
        {
            nomQueued = true;
            Invoke("NomAnimation", 0.1f);
        }
    }

    private void NomAnimation()
    {
        state = MouthState.NOM;
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthNom;
        activeAnimation.SetActive(true);
    }

    private void SpicyReady()
    {
        isSpicyReady = true;
    }

    private void ShootBehavior()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                GamePlayEvents.SendTryVolley();
                break;
            }
        }
    }

    private void Shoot()
    {
        if (state == MouthState.SHOOT)
        {
            if (ammoQueue.Count > 0)
            {
                GameObject instantiatedShot = null;
                ammoQueue.Dequeue();
                if (isSpicyReady)
                {
                    instantiatedShot = Instantiate(SpicyShot, Camera.main.transform.position, Camera.main.transform.rotation);
                    isSpicyReady = false;
                    GamePlayEvents.SendSpicyEnd();
                }
                else
                {
                    instantiatedShot = Instantiate(IceAmmo, Camera.main.transform.position, Camera.main.transform.rotation);
                }
                ScoreEvents.SendSetMultiplier(1.0f);
            }
        }
    }

        private void UpdateMouthState()
    {
        if (state == MouthState.HANDS || state == MouthState.SHOOT)
        {
            if (shotsExisting <= 0)
            {
                AnimationEvents.SendHandsExit();
            }
        }

    }
    private void HandleMouthEnter()
    {
        nomQueued = false;
        Reticle.SetActive(false);
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthIntro;
        activeAnimation.SetActive(true);
        state = MouthState.OPEN;
    }

    private void HandleMouthEntered()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthIdle;
        activeAnimation.SetActive(true);
    }

    private void HandleMouthNommed()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthOutro;
        activeAnimation.SetActive(true);
        shotsExisting = ammoQueue.Count;
        Reticle.SetActive(true);
        AnimationEvents.SendMouthExit();
    }
    private void HandleMouthExited()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = null;
        state = MouthState.SHOOT;
    }

    private void VolleyConfirm()
    {
        if (isSwiping == false)
        {
            isSwiping = true;
            if (leftHandSwipe == true)
            {
                AnimationEvents.SendLeftHandSwipe();
            }
            else
            {
                AnimationEvents.SendRightHandSwipe();
            }
            leftHandSwipe = !leftHandSwipe;
            Invoke("ResetSwipe", 0.1f);
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;
    }

    private void HandleShotDestroyed()
    {
        --shotsExisting;
    }

    private new void OnDestroy()
    {
        AnimationEvents.OnMouthEnter -= HandleMouthEnter;
        AnimationEvents.OnMouthEntered -= HandleMouthEntered;
        AnimationEvents.OnMouthExited -= HandleMouthExited;
        AnimationEvents.OnMouthNommed -= HandleMouthNommed;
        GamePlayEvents.OnShotDestroyed -= HandleShotDestroyed;
        GamePlayEvents.OnSpicyReady -= SpicyReady;
        GamePlayEvents.OnConfirmNom -= DoNom;
        GamePlayEvents.OnConfirmVolley -= VolleyConfirm;
        base.OnDestroy();
    }
}
