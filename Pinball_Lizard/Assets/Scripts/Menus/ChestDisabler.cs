using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDisabler : MonoBehaviour
{
    public GameObject Chest;
    public GameObject Message;
    void Start()
    {
        AnimationEvents.OnChestOpened += DisableChest;
    }


    void Update()
    {

    }

    void DisableChest()
    {
        Chest.SetActive(false);
        Message.SetActive(true);
    }

    private void OnDestroy()
    {
        AnimationEvents.OnChestOpened -= DisableChest;
    }
}
