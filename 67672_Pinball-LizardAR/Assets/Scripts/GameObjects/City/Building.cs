using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject HexStack;
    public GameObject Explosion;
    public HexGrid.HexNode hexNode;
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
            if (collision.gameObject.name.ToLower().Contains("shot"))
            {
                Explode();
                Instantiate(HexStack, gameObject.transform.position, gameObject.transform.localRotation, gameObject.transform.parent);
            }
            if (collision.gameObject.name.ToLower().Contains("spicy"))
            {
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
