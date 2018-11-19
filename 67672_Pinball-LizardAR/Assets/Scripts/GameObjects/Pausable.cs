using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausable : MonoBehaviour {

    public bool isPaused;
	// Use this for initialization
	protected void Start () {
        isPaused = false;
        GamePlayEvents.OnPause += Pause;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Pause()
    {
        isPaused = !isPaused;
    }

    protected void OnDestroy()
    {
        GamePlayEvents.OnPause -= Pause;
    }
}
