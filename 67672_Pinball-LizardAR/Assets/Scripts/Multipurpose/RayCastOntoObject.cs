using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastOntoObject : MonoBehaviour
{
    //TODO: REWRITE THIS FILE
    public Camera cameraForReference;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnTouch();
    }

    private void OnTouch()
    {
        //if (Input.touchCount > 0)
        //{
        //    foreach (Touch touch in Input.touches)
        //    {
        //        Ray ray = cameraForReference.ScreenPointToRay(touch.position);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            //temporary, we'll be getting rid of this
        //            if (touch.phase == TouchPhase.Began)
        //            {
        //                GameObject hitobjectParent = hit.transform.gameObject.transform.parent.gameObject;
        //                if (hitobjectParent.name.ToLower().Contains("hex"))
        //                {
        //                  
        //                }
        //            }
        //            else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //            {
        //                RaycastHit[] draghits = Physics.RaycastAll(ray);
        //                foreach (RaycastHit draghit in draghits)
        //                {
        //                    GameObject hitobject = draghit.transform.gameObject;
        //                    if (hitobject.name.ToLower().Contains("lice") && hitobject.GetComponent<EnemyMovement>().Grabbable == true)
        //                    {
        //                        Vector3 touchScreenSpace = cameraForReference.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.1f));
        //                        Vector3 offset = hitobject.transform.position - touchScreenSpace;
        //                        hitobject.transform.position = Vector3.Lerp(hitobject.transform.position, touchScreenSpace, Time.deltaTime * 2);  
        //                    }

        //                    if (touch.position.y < (Screen.height * 0.2f))
        //                    {
        //                        if(hitobject.name.ToLower().Contains("lice"))
        //                        {
        //                            Destroy(hitobject);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}

