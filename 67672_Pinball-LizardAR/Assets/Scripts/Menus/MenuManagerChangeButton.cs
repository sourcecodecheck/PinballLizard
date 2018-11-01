using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerChangeButton : MonoBehaviour {

    public MenuManager MenuManager;
    public string MenuManagerCommand;
    public bool IsButton;
	// Use this for initialization
	void Start () {
        if (IsButton)
        {
            GetComponent<Button>().onClick.AddListener(ToMainMenu);
        }
        else
        {
            GetComponentInChildren<Button>().onClick.AddListener(ToMainMenu);
        }
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ToMainMenu()
    {
        MenuManager.Invoke(MenuManagerCommand, 0.2f);
    }
}
