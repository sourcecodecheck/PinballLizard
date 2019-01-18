using UnityEngine;

public class HexBehavior : MonoBehaviour
{
    private bool isSelfDestructing;

    void Start()
    {
        isSelfDestructing = false;
    }


    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name.ToLower().Contains("shot") && !isSelfDestructing)
        {
            Rigidbody[] rigidbodies = gameObject.transform.parent.gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
            }
            GetComponent<Rigidbody>().AddForce(collision.transform.position.normalized * 0.2f, ForceMode.Impulse);
            ShotBehavior collidingShot = collision.gameObject.GetComponent<ShotBehavior>();
            collidingShot.HasHitBuilding = true;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Invoke("SelfDestruct", 5.0f);
            isSelfDestructing = true;
        }
    }

    private void SelfDestruct()
    {
        Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }
}
