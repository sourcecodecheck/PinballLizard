using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(GameStart);
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}
