using UnityEngine;

public class HexBehavior : MonoBehaviour
{
    public float TimeToSelfDestruct;

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
            ReleaseRigidbodies();
            GetComponent<Rigidbody>().AddForce(collision.transform.position.normalized, ForceMode.Impulse);
            ShotBehavior collidingShot = collision.gameObject.GetComponent<ShotBehavior>();
            collidingShot.NonReboundDirection = Vector3.Reflect(collidingShot.NonReboundDirection, collision.contacts[0].normal);
            collidingShot.RegenerateRotation();
            collidingShot.Invoke("HitBuilding", 0.1f);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Invoke("SelfDestruct", TimeToSelfDestruct);
            isSelfDestructing = true;
        }
        else if(collision.gameObject.name.ToLower().Contains("collider"))
        {
            ReleaseRigidbodies();
            if (!isSelfDestructing)
            {
                GetComponent<Rigidbody>().AddForce(collision.transform.position.normalized, ForceMode.Impulse);
                Invoke("SelfDestruct", TimeToSelfDestruct);
                isSelfDestructing = true;
            }
            Destroy(collision.gameObject);
        }
    }

    private void ReleaseRigidbodies()
    {
        Rigidbody[] rigidbodies = gameObject.transform.parent.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void SelfDestruct()
    {
        Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }
}
