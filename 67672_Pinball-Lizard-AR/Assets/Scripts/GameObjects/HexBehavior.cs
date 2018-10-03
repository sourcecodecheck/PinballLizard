using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBehavior : MonoBehaviour {

    public Rigidbody rigidbody;
    public MeshCollider collider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("hex") == false)
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            Rigidbody[] rigidbodies = parent.GetComponentsInChildren<Rigidbody>();
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
