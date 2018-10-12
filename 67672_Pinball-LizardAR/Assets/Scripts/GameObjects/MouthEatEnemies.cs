using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouthEatEnemies : MonoBehaviour
{
    private enum AmmoTypes { ICE = 0, FIRE, ATOM, MAX_AMMOTYPES }

    public Sprite OpenMouth;
    public Sprite ClosedMouth;

    public GameObject IceAmmo;
    public GameObject FireAmmo;
    public GameObject AtomAmmo;

    private Queue<AmmoTypes> ammoQueue;
    private bool isOpen;
    // Use this for initialization
    void Start()
    {
        ammoQueue = new Queue<AmmoTypes>();
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        OnTouch();
        UpdateMouthState();
    }

    private void OnTouch()
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
                    if (hitobbjectName.Contains("enemy") && hitobject.GetComponent<EnemyMovement>().Grabbable == true)
                    {
                        Vector3 touchScreenSpace = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.1f));
                        Vector3 offset = hitobject.transform.position - touchScreenSpace;
                        hitobject.transform.position = Vector3.Lerp(hitobject.transform.position, touchScreenSpace, Time.deltaTime * 2);
                        if (touch.position.y < (Screen.height * 0.2f))
                        {
                            Destroy(hitobject);
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
        if (ammoQueue.Count > 0)
        {
            GetComponent<Image>().sprite = ClosedMouth;
            isOpen = false;
        }
        else
        {
            GetComponent<Image>().sprite = OpenMouth;
            isOpen = true;
        }
    }
}
