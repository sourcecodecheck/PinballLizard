using System;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AppCenter.Unity.Crashes;

public class CrashTheGameButton : MonoBehaviour {

    public Button PressToCrash;
	// Use this for initialization
	void Start () {
        PressToCrash.onClick.AddListener(Crash);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Crash()
    {
        try
        {
            throw new Exception("IT'S A CRAAAAAASH!!!!!!");
        }
        catch(Exception generatedCrash)
        {
            Crashes.TrackError(generatedCrash);
        }
    }
}
