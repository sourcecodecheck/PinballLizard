using System.Collections.Generic;
using UnityEngine;

public class MouthBehavior : Pausable
{
    public GameObject MouthIntro;
    public GameObject MouthIdle;
    public GameObject MouthOutro;
    public GameObject MouthNom;
    public GameObject IceAmmo;
    public GameObject FireAmmo;
    public GameObject AtomAmmo;
    public float ShotScale;

    private enum AmmoTypes { ICE = 0, FIRE, ATOM, MAX_AMMOTYPES }
    private enum MouthState { OPEN, NOM, CLOSED}
    private Queue<AmmoTypes> ammoQueue;
    private List<GameObject> taggedEnemies;
    private int shotsExisting;
    private MouthState state;
    private bool isBombPrimed;
    private GameObject activeAnimation;

    // Use this for initialization
    void Awake()
    {
        ammoQueue = new Queue<AmmoTypes>();
        taggedEnemies = new List<GameObject>();
        state = MouthState.OPEN;
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
                if (state == MouthState.OPEN)
                {
                    OpenMouthBehavior();
                }
                else if(state == MouthState.CLOSED)
                {
                    ClosedMouthBehavior();
                }
            }
        }
    }

    private void OpenMouthBehavior()
    {
        bool doNom = false;
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
                    if (hitobbjectName.Contains("enemy") && hitobject.GetComponent<EnemyBehavior>().Grabbable == true && taggedEnemies.Contains(hitobject.gameObject) == false)
                    {
                        taggedEnemies.Add(hitobject.gameObject);
                    }
                }
                foreach (GameObject taggedEnemy in taggedEnemies)
                {
                    Vector3 touchScreenSpace = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.1f));
                    touchScreenSpace.z = taggedEnemy.transform.position.z;
                    taggedEnemy.transform.position = touchScreenSpace;//Vector3.Lerp(taggedEnemy.transform.position, touchScreenSpace, 0.8f);
                    if (touch.position.y < (Screen.height * 0.2f))
                    {
                        doNom = true;
                        break;
                    }
                }
            }
            if (doNom == true)
            {
                DoNom();
                break;
            }
        }
    }
    private void DoNom()
    {
        state = MouthState.NOM;
        foreach (GameObject taggedEnemy in taggedEnemies)
        {
            taggedEnemy.SetActive(false);
            TrackingEvents.SendBugEaten();
            if (taggedEnemy.name.ToLower().Contains("ice"))
            {
                ammoQueue.Enqueue(AmmoTypes.ICE);
            }
            else if (taggedEnemy.name.ToLower().Contains("fire"))
            {
                ammoQueue.Enqueue(AmmoTypes.FIRE);
            }
            else if (taggedEnemy.name.ToLower().Contains("atom"))
            {
                ammoQueue.Enqueue(AmmoTypes.ATOM);
            }
        }
        shotsExisting = ammoQueue.Count;
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
                    instantiatedShot.transform.localScale *= ShotScale;
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
    }

    private void UpdateMouthState()
    {
        if(state == MouthState.CLOSED)
        {
            if (shotsExisting <= 0)
            {
                AnimationEvents.SendHandsExit();
            }
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
        state = MouthState.OPEN;
        foreach(GameObject enemy in taggedEnemies)
        {
            Destroy(enemy);
        }
        taggedEnemies.Clear();
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
        state = MouthState.CLOSED;
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
        base.OnDestroy();
    }
}
