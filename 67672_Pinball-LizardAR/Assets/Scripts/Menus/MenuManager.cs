using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    
    public GameObject MainMenuButtons;
    public GameObject SettingsButton;
    public Canvas MenuParent;

    private List<GameObject> menuObjects;
	// Use this for initialization
	void Start () {
        menuObjects = new List<GameObject>();
        LoadMainMenu();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void LoadMainMenu()
    {
        menuObjects.Add(Instantiate(MainMenuButtons, MenuParent.transform));
        menuObjects.Add(Instantiate(SettingsButton, MenuParent.transform));
    }


    private void UnloadMenu()
    {
        foreach(GameObject menuObject in menuObjects)
        {
            Destroy(menuObject);
        }
        menuObjects.Clear();
    }
}
