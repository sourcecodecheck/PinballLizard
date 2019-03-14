using UnityEngine;

public class ShotBehavior : Pausable
{
    public bool HasHitBuilding;
    public bool IsSpicy;
    public GameObject Explosion;
    public bool IsNonAR;
    public float MovementSpeed;
    public float RotationMin;
    public float RotationMax;
    public Vector3 NonReboundDirection;

    private float deathDistance;
    private float deathDistanceDerivedSpeed;
    private bool isBeingDestroyed;
    private bool isVolleyable;
    private Renderer shotRenderer;
    private Vector3 rotationVector;
    private float rotationSpeed;
    
    new void Start()
    {
        rotationVector = Vector3.zero;
        GamePlayEvents.OnTryVolley += Volley;
        HasHitBuilding = false;
        base.Start();
        GameObject city = GameObject.Find("City");
        if(city != null)
        {
            deathDistance = Vector3.Distance(city.transform.position, Camera.main.transform.position);
        }
        else
        {
            deathDistance = 5;
        }
        isBeingDestroyed = false;
        deathDistanceDerivedSpeed = deathDistance * MovementSpeed;
        shotRenderer = GetComponentInChildren<Renderer>();
        NonReboundDirection = Camera.main.transform.forward;
        RegenerateRotation();
        if(IsNonAR)
        {
            deathDistance *= 1.5f;
        }
    }
    public void HitBuilding()
    {
        HasHitBuilding = true;
        RegenerateRotation();
    }
    public void RegenerateRotation()
    {
        rotationVector = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        rotationSpeed = Random.Range(RotationMin, RotationMax);
    }

    private void Update()
    {
        float distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        if (!isPaused)
        {
            if (HasHitBuilding)
            {
                Vector3 nextPosition = Vector3.MoveTowards(transform.position, Camera.main.transform.position,
                   deathDistanceDerivedSpeed * Time.deltaTime);
                if(Vector3.Distance(gameObject.transform.position, nextPosition) < 0.0001f && isBeingDestroyed == false)
                {
                    AnimationEvents.SendMissEnter();
                    SelfDestruct();
                    return;
                }
                transform.position = nextPosition;
            }
            else
            {
                gameObject.transform.position += NonReboundDirection * deathDistanceDerivedSpeed * Time.deltaTime;
            }
            gameObject.transform.Rotate(rotationVector, rotationSpeed);
        }
        if(isBeingDestroyed == false)
        {
            if(distanceFromCamera > deathDistance || (shotRenderer.isVisible == false && HasHitBuilding == true))
            {
                SelfDestruct();
            }
        }
        if(distanceFromCamera <= (deathDistance * 0.2f))
        {
            isVolleyable = true;
        }
        else
        {
            isVolleyable = false;
        }
    }

    public void Volley()
    {
        if (HasHitBuilding == true && isVolleyable == true)
        {
            rotationVector = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position,
                 deathDistanceDerivedSpeed * Time.deltaTime);
            NonReboundDirection = Camera.main.transform.forward;
            RegenerateRotation();
            HasHitBuilding = false;
            GamePlayEvents.SendConfirmVolley();
        }
        else
        {
            AnimationEvents.SendMissEnter();
        }
    }
    private void SelfDestruct()
    {
        Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
        isBeingDestroyed = true;
        Destroy(gameObject);
        if (IsSpicy == false)
        {
            TrackingEvents.SendBugEaten();
        }
    }

    private new void OnDestroy()
    {
        GamePlayEvents.OnTryVolley -= Volley;
        if (IsSpicy == false)
        {
            GamePlayEvents.SendShotDestroyed();
        }
        base.OnDestroy();
    }
}

