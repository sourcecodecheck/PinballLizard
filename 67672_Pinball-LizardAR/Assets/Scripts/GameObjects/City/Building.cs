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
    
    void Start()
    {
        isSelfDestructing = false;
        GamePlayEvents.OnBombDetonated += Explode;
    }
    
    void Update()
    {

    }

    public void Explode()
    {
        if (!isSelfDestructing)
        {
            isSelfDestructing = true;
            ScoreEvents.SendAddMultiplier(0.1f * StackCount);
            ScoreEvents.SendAddScore(StackCount);
            GameObject explosion = Instantiate(Explosion, gameObject.transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));
            explosion.transform.localScale = transform.localScale * 200f;
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
                Explode();
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
                Instantiate(EmptySpace, gameObject.transform.parent);
                Destroy(collision.gameObject);
                hexNode.SpreadExplosion();
                Explode();
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
