using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GamePlayEvents.OnPause += OnUnPause;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnUnPause()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GamePlayEvents.OnPause -= OnUnPause;
    }
}
