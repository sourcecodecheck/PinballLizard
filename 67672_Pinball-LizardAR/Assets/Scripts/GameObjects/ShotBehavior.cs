using UnityEngine;

public class ShotBehavior : Pausable
{
    public bool HasHitBuilding;
    public float Speed;
    public int Life;
    public GameObject Explosion;
    public float DeathDistance;

    private float deathDistanceDerivedSpeed;
    private bool isBeingDestroyed;

    // Use this for initialization
    new void Start()
    {
        HasHitBuilding = false;
        base.Start();
        GameObject city = GameObject.Find("City");
        if(city != null)
        {
            DeathDistance = Vector3.Distance(city.transform.position, Camera.main.transform.position) * 1.5f;
        }
        else
        {
            DeathDistance = 5;
        }
        isBeingDestroyed = false;
        deathDistanceDerivedSpeed = DeathDistance * 0.1f;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (HasHitBuilding)
            {
                transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position,
                   deathDistanceDerivedSpeed * Time.deltaTime);
            }
            else
            {
                gameObject.transform.position += gameObject.transform.forward * deathDistanceDerivedSpeed * Time.deltaTime;
            }
            if (Life <= 0)
            {
                Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        if((Vector3.Distance(transform.position, Camera.main.transform.position) > DeathDistance 
            || Vector3.Distance(transform.position, Camera.main.transform.position) <= 0) 
            && isBeingDestroyed == false ) 
        {
            isBeingDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void Volley()
    {
        if (HasHitBuilding)
        {
            HasHitBuilding = false;
            --Life;
        }
    }
    private new void OnDestroy()
    {
        GamePlayEvents.SendShotDestroyed();
        base.OnDestroy();
    }
}

