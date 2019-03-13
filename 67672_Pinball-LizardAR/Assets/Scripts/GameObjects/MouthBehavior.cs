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
    public GameObject SpicyReticle;
    public GameObject SpicyShot;
    public Button[] ReticleButtons;
    public float VolleyInterval;

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
        foreach (Button button in ReticleButtons)
        {
            button.onClick.AddListener(Shoot);
        }
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
                else if (state == MouthState.HANDS)
                {
                    HandsBehavior();
                }
            }
        }
    }

    private void OpenMouthBehavior()
    {
        //bool doNom = false;
        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit[] dragHits = Physics.RaycastAll(ray);
                foreach (RaycastHit dragHit in dragHits)
                {
                    GameObject hitobject = dragHit.transform.gameObject;
                    string hitobbjectName = hitobject.name.ToLower();
                    if (hitobbjectName.Contains("enemy"))
                    {
                        GamePlayEvents.SendTryNom(hitobject.GetInstanceID());
                        //doNom = true;
                    }
                }
            }
            
        }
        //if (doNom == false && Input.touchCount > 0)
        //{
        //    AnimationEvents.SendMissEnter();
        //}

    }
    private void DoNom()
    {
        ammoQueue.Enqueue(AmmoTypes.ICE);
        if (nomQueued == false)
        {
            nomQueued = true;
            Invoke("NomAnimation", 0.1f);
        }
    }

    private void NomAnimation()
    {
        AudioEvents.SendPlayNom();
        state = MouthState.NOM;
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthNom;
        AnimationEvents.SendMouthNom();
        activeAnimation.SetActive(true);
    }

    private void SpicyReady()
    {
        isSpicyReady = true;
        if(state == MouthState.SHOOT)
        {
            Reticle.SetActive(false);
            SpicyReticle.SetActive(true);
        }
        else if(state == MouthState.OPEN)
        {
            state = MouthState.SHOOT;
            HandleMouthNommed();
            SpicyReticle.SetActive(true);
        }
        
    }

    private void HandsBehavior()
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
            if (ammoQueue.Count > 0 || isSpicyReady == true)
            {
                if (isSpicyReady)
                {
                    Instantiate(SpicyShot, Camera.main.transform.position, Camera.main.transform.rotation);
                    isSpicyReady = false;
                    GamePlayEvents.SendSpicyEnd();
                    SpicyReticle.SetActive(false);
                    Reticle.SetActive(true);
                }
                else
                {
                    ammoQueue.Dequeue();
                    Instantiate(IceAmmo, Camera.main.transform.position, Camera.main.transform.rotation);
                    state = MouthState.HANDS;
                    AnimationEvents.SendHandsEnter();
                    Reticle.SetActive(false);
                }
                TrackingEvents.SendBuildVolleyActionStep2(new CityVolleyAction()
                {
                    VolleyAction = "shoot",
                    VolleySource = "player",
                }, EventNames.VolleyAction);
                AudioEvents.SendPlaySpit();
                ScoreEvents.SendSetMultiplier(1.0f);
               
            }
        }
    }

    private void UpdateMouthState()
    {
        if (state == MouthState.HANDS || state == MouthState.SHOOT)
        {
            if(state == MouthState.SHOOT)
            {
                AnimationEvents.SendHandsExit();
            }
            if (shotsExisting <= 0 )
            {
                AnimationEvents.SendHandsExit();
                if (isSpicyReady == false)
                {
                    AnimationEvents.SendMouthEnter();
                }
                else
                {
                    Reticle.SetActive(false);
                    SpicyReticle.SetActive(true);
                }
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
        if (isSpicyReady == true)
        {
            SpicyReticle.SetActive(true);
        }
        else
        {
            Reticle.SetActive(true);
        }
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
        GamePlayEvents.SendUpdateAmmoCount(ammoQueue.Count);
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
            TrackingEvents.SendBuildVolleyActionStep2(new CityVolleyAction()
            {
                VolleyAction = "volley",
                VolleySource = "player",
            }, EventNames.VolleyAction);
            leftHandSwipe = !leftHandSwipe;
            Invoke("ResetSwipe", VolleyInterval);
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;
    }

    private void HandleShotDestroyed()
    {
        --shotsExisting;
        //AnimationEvents.SendHandsExit();
        Reticle.SetActive(true);
        state = MouthState.SHOOT;
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
