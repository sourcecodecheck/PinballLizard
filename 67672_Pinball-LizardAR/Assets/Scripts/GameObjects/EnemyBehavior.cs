using UnityEngine;

public class EnemyBehavior : Pausable
{
    public bool KeepMoving;
    public bool Grabbable;

    private float moveStep;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        Grabbable = false;
        KeepMoving = true;
        moveStep = Vector3.Distance(transform.position, Camera.main.transform.position) * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (KeepMoving)
            {
                transform.parent = null;
                float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position), Time.deltaTime);
                if (distanceToCamera > moveStep * 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, moveStep * Time.deltaTime);
                }
                else
                {
                    Grabbable = true;
                }
            }
        }
    }
    new private void OnDestroy()
    {
        base.OnDestroy();
    }
}
