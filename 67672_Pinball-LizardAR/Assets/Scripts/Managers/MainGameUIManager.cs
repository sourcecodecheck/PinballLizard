using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameUIManager : MonoBehaviour {

    public GameObject PauseMenu;
    public Canvas MenuParent;
	// Use this for initialization
	void Start () {
        GamePlayEvents.OnLoadPauseMenu += LoadPauseMenu;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadPauseMenu()
    {
        Instantiate(PauseMenu, MenuParent.transform);
    }
    private void OnDestroy()
    {
        GamePlayEvents.OnLoadPauseMenu -= LoadPauseMenu;
    }
}
