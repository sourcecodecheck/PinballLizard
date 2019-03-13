using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject HexStack;
    public GameObject Explosion;
    public GameObject ARCollider;
    public GameObject NonARCollider;
    public GameObject EmptySpace;

    public HexNode hexNode;
    public int StackCount;


    private bool isSelfDestructing;
    private Vector3 collisionLocation;
    
    void Start()
    {
        collisionLocation = new Vector3();
        isSelfDestructing = false;
        GamePlayEvents.OnBombDetonated += Explode;
    }
    
    void Update()
    {

    }

    public void Explode(string damageSource)
    {
        if (!isSelfDestructing)
        {
            isSelfDestructing = true;
            ScoreEvents.SendAddMultiplier(0.1f * StackCount);
            ScoreEvents.SendAddScore(StackCount);
            GameObject explosion = Instantiate(Explosion, gameObject.transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));
            explosion.transform.localScale = transform.localScale * 200f;
            TrackingEvents.SendBuildingDestroyedStep2(new CityBuildingDestroyed()
                {
                    DamageLoactionX = collisionLocation.x,
                    DamageLoactionY = collisionLocation.y,
                    DamageLoactionZ = collisionLocation.z,
                    DamageType = damageSource,
                    ScoreBaseValue = StackCount
                }, EventNames.BuildingDestroyed);
            Handheld.Vibrate();
            Invoke("DestroySelf", 0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSelfDestructing)
        {
            string colliderName = collision.gameObject.name.ToLower();
            if (colliderName.Contains("shot"))
            {
                Explode("bug");
                collisionLocation = collision.transform.position;
                GameObject stack = Instantiate(HexStack, gameObject.transform.position, gameObject.transform.localRotation, gameObject.transform.parent);
                if (colliderName.Contains("non"))
                {
                    Instantiate(NonARCollider, stack.transform);
                }
                else
                {
                    Instantiate(ARCollider, stack.transform);
                }
                
            }
            if (colliderName.Contains("spicy"))
            {
                collisionLocation = collision.transform.position;
                Instantiate(EmptySpace, gameObject.transform.parent);
                Destroy(collision.gameObject);
                if (hexNode != null)
                {
                    hexNode.SpreadExplosion("spicy-collateral");
                }
                Explode("spicy-source");
            }
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GamePlayEvents.OnBombDetonated -= Explode;
        TrackingEvents.SendBuildingDestroyed();
    }
}
