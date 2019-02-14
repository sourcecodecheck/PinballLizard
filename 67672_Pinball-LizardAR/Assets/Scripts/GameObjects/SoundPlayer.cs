using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public enum SoundCategory { BUILDING, SMACK, BOOP};
    public SoundCategory Category;
    public AudioClip[] Sounds;
    public AudioSource SoundSource;

    // Use this for initialization
    void Start()
    {
        switch(Category)
        {
            case SoundCategory.BUILDING:
                AudioEvents.OnPlayBuildingCollapse += PlaySound;
                break;
            case SoundCategory.SMACK:
                AudioEvents.OnPlayBugSmack += PlaySound;
                break;
            case SoundCategory.BOOP:
                AudioEvents.OnPlayMenuBoop += PlaySound;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void PlaySound()
    {
        int soundSelected = Random.Range(0, Sounds.Length - 1);
        SoundSource.PlayOneShot(Sounds[soundSelected]);
        
    }
    private void OnDestroy()
    {
        switch (Category)
        {
            case SoundCategory.BUILDING:
                AudioEvents.OnPlayBuildingCollapse -= PlaySound;
                break;
            case SoundCategory.SMACK:
                AudioEvents.OnPlayBugSmack -= PlaySound;
                break;
            case SoundCategory.BOOP:
                AudioEvents.OnPlayMenuBoop -= PlaySound;
                break;
        }
    }
}
