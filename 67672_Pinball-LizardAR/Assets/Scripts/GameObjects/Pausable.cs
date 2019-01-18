using UnityEngine;

public class Pausable : MonoBehaviour {

    public bool isPaused;
	
	protected void Start () {
        isPaused = false;
        GamePlayEvents.OnPause += Pause;
	}
	
	
	void Update () {
		
	}

    void Pause(bool loadPauseMenu)
    {
        isPaused = !isPaused;
    }

    protected void OnDestroy()
    {
        GamePlayEvents.OnPause -= Pause;
    }
}
