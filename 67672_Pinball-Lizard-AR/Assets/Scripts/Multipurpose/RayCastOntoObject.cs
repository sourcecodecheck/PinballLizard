using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastOntoObject : MonoBehaviour
{

    public GameObject particle;
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
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            foreach (Touch  touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = cameraForReference.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Rigidbody[] rigidbodies = hit.transform.parent.gameObject.GetComponentsInChildren<Rigidbody>();
                        foreach (Rigidbody rigidbody in rigidbodies)
                        {
                            rigidbody.constraints = RigidbodyConstraints.None;
                        }
                        hit.rigidbody.AddForce(ray.direction, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
