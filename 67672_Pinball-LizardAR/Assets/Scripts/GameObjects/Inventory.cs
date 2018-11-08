using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public int BreathWeaponCount;
    public int SkipCount;
    public int InstaBombCount;
    public int MayhemCount;
    public int BugBucksCount;
    public int AnimosityCount;
    public int PlayerLevel;
    public int ExperienceCount;
    public int ExperienceToNextLevel;
  
	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateFromPlayFab", 5.0f, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateFromPlayFab()
    {

    }
}
