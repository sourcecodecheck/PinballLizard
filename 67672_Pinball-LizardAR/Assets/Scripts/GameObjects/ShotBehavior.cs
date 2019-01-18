using UnityEngine;

public class ShotBehavior : Pausable
{
    public bool HasHitBuilding;
    public float Speed;
    public int Life;
    public GameObject Explosion;
    public float DeathDistance;
    public float Scale;

    private float deathDistanceDerivedSpeed;
    private bool isBeingDestroyed;
    private bool isVolleyable;
    
    new void Start()
    {
        GamePlayEvents.OnTryVolley += Volley;
        HasHitBuilding = false;
        base.Start();
        GameObject city = GameObject.Find("City");
        if(city != null)
        {
            DeathDistance = Vector3.Distance(city.transform.position, Camera.main.transform.position);
        }
        else
        {
            DeathDistance = 5;
        }
        isBeingDestroyed = false;
        deathDistanceDerivedSpeed = DeathDistance * 0.3f;
        
    }

    private void Update()
    {
        float distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
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
        if((distanceFromCamera > DeathDistance 
            || distanceFromCamera < 0) 
            && isBeingDestroyed == false ) 
        {
            isBeingDestroyed = true;
            Destroy(gameObject);
        }
        if(distanceFromCamera <= (DeathDistance * 0.2f))
        {
            isVolleyable = true;
        }
    }

    public void Volley()
    {
        if (HasHitBuilding == true && isVolleyable == true)
        {
            HasHitBuilding = false;
            GamePlayEvents.SendConfirmVolley();
        }
    }
    private new void OnDestroy()
    {
        GamePlayEvents.OnTryVolley -= Volley;
        GamePlayEvents.SendShotDestroyed();
        base.OnDestroy();
    }
}

