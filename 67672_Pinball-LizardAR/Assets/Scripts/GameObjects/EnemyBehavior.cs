using System;
using UnityEngine;

public class EnemyBehavior : Pausable
{
    public bool WillKeepMoving;
    public bool IsGrabbable;

    private float moveStep;
    private bool isBeingNommed;
    
    new void Start()
    {
        base.Start();
        IsGrabbable = false;
        WillKeepMoving = true;
        isBeingNommed = false;
        moveStep = Vector3.Distance(transform.position, Camera.main.transform.position) * 0.3f;
        GamePlayEvents.OnTryNom += TryNom;
    }

    private void TryNom()
    {
       if(isBeingNommed == false && IsGrabbable == true)
       {
            isBeingNommed = true;
            GamePlayEvents.SendConfirmNom();
            Destroy(gameObject);
       }
    }

    void Update()
    {
        if (!isPaused)
        {
            if (WillKeepMoving)
            {
                transform.parent = null;
                float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(Camera.main.transform.position - transform.position), Time.deltaTime);
                if (distanceToCamera > moveStep * Time.deltaTime *10f)
                {
                    transform.position = 
                        Vector3.MoveTowards(transform.position, Camera.main.transform.position, moveStep * Time.deltaTime);
                }
                if (distanceToCamera <= moveStep )
                {
                    IsGrabbable = true;
                }
            }
            else
            {
                IsGrabbable = true;
            }
        }
    }
    new private void OnDestroy()
    {
        GamePlayEvents.OnTryNom -= TryNom;
        base.OnDestroy();
    }
}
