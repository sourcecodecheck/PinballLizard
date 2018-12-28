using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float MovementSpeed;
    public bool KeepMoving;
    public bool Grabbable;

    // Use this for initialization
    void Start()
    {
        Grabbable = false;
        KeepMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (KeepMoving)
        {
            float moveStep = MovementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position), Time.deltaTime);
            float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
            if (distanceToCamera > 0.1f) 
            {
                transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, moveStep * Time.deltaTime);
            }
            if (distanceToCamera >= 0.09f && distanceToCamera < 0.15f)
            {
                Grabbable = true;
            }
        }
    }
}
