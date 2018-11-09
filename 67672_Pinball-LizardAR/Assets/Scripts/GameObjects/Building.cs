using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject HexStack;

    private bool isSelfDestructing;
    // Use this for initialization
    void Start()
    {
        isSelfDestructing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower().Contains("shot") && !isSelfDestructing)
        {
            isSelfDestructing = true;
            Instantiate(HexStack, gameObject.transform.position, gameObject.transform.localRotation);
            Invoke("DestroySelf", 0.1f);
        }

    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        TrackingEvents.SendBuildingDestroyed();
    }
}
