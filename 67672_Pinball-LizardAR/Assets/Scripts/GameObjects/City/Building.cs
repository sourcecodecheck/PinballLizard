using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject HexStack;
    public HexGrid.HexNode hexNode;
    public int StackCount;

    private bool isSelfDestructing;
    // Use this for initialization
    void Start()
    {
        isSelfDestructing = false;
        GamePlayEvents.OnBombDetonated += Explode;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Explode()
    {
        if (!isSelfDestructing)
        {
            isSelfDestructing = true;
            GameObject stack = Instantiate(HexStack, gameObject.transform.position, gameObject.transform.localRotation);
            stack.transform.localScale = transform.localScale;
            ScoreEvents.SendAddMultiplier(0.1f * StackCount);
            ScoreEvents.SendAddScore(StackCount);
            Invoke("DestroySelf", 0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower().Contains("shot"))
        {
            Explode();
        }
        if (collision.gameObject.name.ToLower().Contains("spicy"))
        {
            Destroy(collision.gameObject);
            hexNode.SpreadExplosion();
            Explode();
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
