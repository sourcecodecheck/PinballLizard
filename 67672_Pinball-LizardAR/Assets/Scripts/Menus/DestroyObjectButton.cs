using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObjectButton : MonoBehaviour {

    public GameObject ObjectToDestroy;
    public Button OnPress;
	// Use this for initialization
	void Start () {
        OnPress.onClick.AddListener(DestroyObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void DestroyObject()
    {
        Destroy(ObjectToDestroy);
    }
}
