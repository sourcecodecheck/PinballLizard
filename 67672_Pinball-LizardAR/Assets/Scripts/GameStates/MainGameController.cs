using UnityEngine;

public class MainGameController : MonoBehaviour {
    
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
