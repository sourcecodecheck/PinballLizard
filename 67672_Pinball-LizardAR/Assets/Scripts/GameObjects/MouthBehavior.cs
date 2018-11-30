using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouthBehavior: Pausable
{
    public GameObject MouthIntro;
    public GameObject MouthIdle;
    public GameObject MouthOutro;
    public GameObject MouthNom;
    public GameObject IceAmmo;
    public GameObject FireAmmo;
    public GameObject AtomAmmo;

    private enum AmmoTypes { ICE = 0, FIRE, ATOM, MAX_AMMOTYPES }
    private Queue<AmmoTypes> ammoQueue;
    private List<GameObject> eatenEnemies;
    private int shotsExisting;
    private bool isOpen;
    private bool isBombPrimed;
    private GameObject activeAnimation;

    // Use this for initialization
    new void Start()
    {
        ammoQueue = new Queue<AmmoTypes>();
        eatenEnemies = new List<GameObject>();
        isOpen = true;
        isBombPrimed = false;
        AnimationEvents.OnMouthEnter += HandleMouthEnter;
        AnimationEvents.OnMouthEntered += HandleMouthEntered;
        AnimationEvents.OnMouthExited += HandleMouthExited;
        AnimationEvents.OnMouthNommed += HandleMouthNommed;
        GamePlayEvents.OnShotDestroyed += HandleShotDestroyed;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        OnTouch();
        UpdateMouthState();
    }

    public void PrimeBomb()
    {
        isBombPrimed = true;
    }

    private void OnTouch()
    {
        if (!isPaused)
        {
            if (Input.touchCount > 0)
            {
                if (isOpen)
                {
                    OpenMouthBehavior();
                }
                else
                {
                    ClosedMouthBehavior();
                }
            }
        }
    }

    private void OpenMouthBehavior()
    {
        foreach (Touch touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                RaycastHit[] dragHits = Physics.RaycastAll(ray);
                foreach (RaycastHit dragHit in dragHits)
                {
                    GameObject hitobject = dragHit.transform.gameObject;
                    string hitobbjectName = hitobject.name.ToLower();
                    if (hitobbjectName.Contains("enemy") && hitobject.GetComponent<EnemyBehavior>().Grabbable == true)
                    {
                        Vector3 touchScreenSpace = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.1f));
                        Vector3 offset = hitobject.transform.position - touchScreenSpace;
                        hitobject.transform.position = Vector3.Lerp(hitobject.transform.position, touchScreenSpace, Time.deltaTime * 3);
                        if (touch.position.y < (Screen.height * 0.2f))
                        {

                                DoNom();
                            hitobject.SetActive(false);
                            eatenEnemies.Add(hitobject);
                            TrackingEvents.SendBugEaten();
                            if (hitobbjectName.Contains("ice"))
                            {
                                ammoQueue.Enqueue(AmmoTypes.ICE);
                            }
                            else if (hitobbjectName.Contains("fire"))
                            {
                                ammoQueue.Enqueue(AmmoTypes.FIRE);
                            }
                            else if (hitobbjectName.Contains("atom"))
                            {
                                ammoQueue.Enqueue(AmmoTypes.ATOM);
                            }
                        }
                    }
                }
            }
        }
    }
    private void DoNom()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthNom;
        activeAnimation.SetActive(true);
    }

    private void ClosedMouthBehavior()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && touch.position.y < (Screen.height * 0.3f))
            {
                if (ammoQueue.Count > 0)
                {
                    AmmoTypes shot = ammoQueue.Dequeue();
                    GameObject instantiatedShot = null;
                    switch (shot)
                    {
                        case AmmoTypes.ICE:
                            instantiatedShot = Instantiate(IceAmmo, Camera.main.transform.position, Camera.main.transform.rotation);
                            break;
                        case AmmoTypes.FIRE:
                            instantiatedShot = Instantiate(FireAmmo, Camera.main.transform.position, Camera.main.transform.rotation);
                            break;
                        case AmmoTypes.ATOM:
                            instantiatedShot = Instantiate(AtomAmmo, Camera.main.transform.position, Camera.main.transform.rotation);
                            break;
                    }
                    if (isBombPrimed)
                    {
                        instantiatedShot.GetComponent<ShotBehavior>().Life = 0;
                        isBombPrimed = false;
                    }
                    ScoreEvents.SendSetMultiplier(1.0f);
                }
                else
                {
                    break;
                }
            }
        }
        //if(ammoQueue.Count <= 0)
        //{
        //    AnimationEvents.SendLeftHandExit();
        //    AnimationEvents.SendRightHandExit();
        //}
    }

    private void UpdateMouthState()
    {
        if (ammoQueue.Count > 0)
        {

            isOpen = false;
        }
        else
        {
            
            foreach (GameObject enemy in eatenEnemies)
            {
                Destroy(enemy);
            }
            eatenEnemies.Clear();
        }
    }
    private void HandleMouthEnter()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = MouthIntro;
        activeAnimation.SetActive(true);
        isOpen = true;
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
        AnimationEvents.SendMouthExit();
        shotsExisting = ammoQueue.Count;
    }
    private void HandleMouthExited()
    {
        if (activeAnimation != null)
        {
            activeAnimation.SetActive(false);
        }
        activeAnimation = null;
    }

    private void HandleShotDestroyed()
    {
        --shotsExisting;
        if (shotsExisting <= 0)
        {
            AnimationEvents.SendHandsExit();
        }
    }

    private new void OnDestroy()
    {
        AnimationEvents.OnMouthEnter -= HandleMouthEnter;
        AnimationEvents.OnMouthEntered -= HandleMouthEntered;
        AnimationEvents.OnMouthExited -= HandleMouthExited;
        AnimationEvents.OnMouthNommed -= HandleMouthNommed;
        GamePlayEvents.OnShotDestroyed -= HandleShotDestroyed;
        base.OnDestroy();
    }
}
