﻿using UnityEngine;

public class HexBehavior : MonoBehaviour
{
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

        if (collision.gameObject.name.ToLower().Contains("shot"))
        {
            Rigidbody[] rigidbodies = gameObject.transform.parent.gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
            }
            GetComponent<Rigidbody>().AddForce(collision.transform.position.normalized, ForceMode.Impulse);
            ShotBehavior collidingShot = collision.gameObject.GetComponent<ShotBehavior>();
            collidingShot.HasHitBuilding = true;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (!isSelfDestructing)
            {
                Invoke("SelfDestruct", 5.0f);
                isSelfDestructing = true;
            }
        }
    }

    private void SelfDestruct()
    {
        Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }
}
