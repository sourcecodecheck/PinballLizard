using UnityEngine;

public class EnemyBehavior : Pausable
{
    public float Rotation;

    private bool isBeingNommed;
    private float accumulator;

    new void Start()
    {
        accumulator = 0;
        base.Start();
        isBeingNommed = false;
        GamePlayEvents.OnTryNom += TryNom;
    }

    private void TryNom(int instanceId)
    {
       if(isBeingNommed == false && gameObject.GetInstanceID() == instanceId)
       {
            isBeingNommed = true;
            TrackingEvents.SendBuildBugEatenStep2(new CityBugEaten()
            {
                BugLocationX = gameObject.transform.position.x,
                BugLocationY = gameObject.transform.position.y,
                BugLocationZ = gameObject.transform.position.z
            }, EventNames.BugEaten);
            GamePlayEvents.SendConfirmNom();
            Invoke("SelfDestruct", 0.2f);
       }
    }

    void Update()
    {
        if (!isPaused)
        {
            if (accumulator > 4000000)
                accumulator = 0f;
            transform.RotateAround(gameObject.transform.parent.position, new Vector3(0f, 1f, 0f), Rotation);
        }
    }
    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    new private void OnDestroy()
    {
        GamePlayEvents.OnTryNom -= TryNom;
        base.OnDestroy();
    }
}
