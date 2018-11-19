using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {

    public int SceneToChangeTo;
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(GameStart);
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void GameStart()
    {
        SceneManager.LoadScene(SceneToChangeTo);
    }
}
