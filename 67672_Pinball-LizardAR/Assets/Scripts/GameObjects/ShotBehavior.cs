using UnityEngine;

public class ShotBehavior : MonoBehaviour
{
    public float Speed;
    public bool HasHitBuilding;
    public int Life;

    // Use this for initialization
    void Start()
    {
        HasHitBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasHitBuilding)
        {
            gameObject.transform.position += gameObject.transform.forward * Speed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, Speed * Time.deltaTime * 0.15f);
        }
        if(Life <= 0)
        {
            //TODO: Do big boom thing
            Destroy(gameObject);
        }
    }
}

