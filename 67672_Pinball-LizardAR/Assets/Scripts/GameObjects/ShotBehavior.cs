using UnityEngine;

public class ShotBehavior : Pausable
{
    public float Speed;
    public bool HasHitBuilding;
    public int Life;
    public GameObject Explosion;

    // Use this for initialization
    new void Start()
    {
        HasHitBuilding = false;
        base.Start();
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (HasHitBuilding)
            {
                transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position,
                    Speed * Time.deltaTime * 0.15f);
            }
            else
            {
                gameObject.transform.position += gameObject.transform.forward * Speed * Time.deltaTime;
            }
            if (Life <= 0)
            {
                Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
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
        base.OnDestroy();
    }
}

