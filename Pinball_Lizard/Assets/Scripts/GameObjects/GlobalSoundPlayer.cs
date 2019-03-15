using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundPlayer : MonoBehaviour
{
    public enum SoundCategory { BUILDING, SMACK, BOOP1, BOOP2, BOOP3, BOOP4, STARTGAME, UP, DOWN, GETITEM, NOM, POWERUP, SPIT,_CHEST_OPEN};
    public SoundCategory Category;
    public AudioClip[] Sounds;
    public AudioSource SoundSource;

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
            case SoundCategory.BOOP1:
                AudioEvents.OnPlayMenuBoop1 += PlaySound;
                break;
            case SoundCategory.BOOP2:
                AudioEvents.OnPlayMenuBoop2 += PlaySound;
                break;
            case SoundCategory.BOOP3:
                AudioEvents.OnPlayMenuBoop3 += PlaySound;
                break;
            case SoundCategory.BOOP4:
                AudioEvents.OnPlayMenuBoop4 += PlaySound;
                break;
            case SoundCategory.GETITEM:
                AudioEvents.OnPlayItemGet += PlaySound;
                break;
            case SoundCategory.UP:
                AudioEvents.OnPlayUp += PlaySound;
                break;
            case SoundCategory.DOWN:
                AudioEvents.OnPlayDown += PlaySound;
                break;
            case SoundCategory.STARTGAME:
                AudioEvents.OnPlayGameStart += PlaySound;
                break;
            case SoundCategory.NOM:
                AudioEvents.OnPlayNom += PlaySound;
                break;
            case SoundCategory.SPIT:
                AudioEvents.OnPlaySpit += PlaySound;
                break;
            case SoundCategory.POWERUP:
                AudioEvents.OnPlayPowerUp += PlaySound;
                break;
            case SoundCategory._CHEST_OPEN:
                AudioEvents.OnPlayChestOpen += PlaySound;
                break;
        }
    }

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
            case SoundCategory.BOOP1:
                AudioEvents.OnPlayMenuBoop1 -= PlaySound;
                break;
            case SoundCategory.BOOP2:
                AudioEvents.OnPlayMenuBoop2 -= PlaySound;
                break;
            case SoundCategory.BOOP3:
                AudioEvents.OnPlayMenuBoop3 -= PlaySound;
                break;
            case SoundCategory.BOOP4:
                AudioEvents.OnPlayMenuBoop4 -= PlaySound;
                break;
            case SoundCategory.GETITEM:
                AudioEvents.OnPlayItemGet -= PlaySound;
                break;
            case SoundCategory.UP:
                AudioEvents.OnPlayUp -= PlaySound;
                break;
            case SoundCategory.DOWN:
                AudioEvents.OnPlayDown -= PlaySound;
                break;
            case SoundCategory.STARTGAME:
                AudioEvents.OnPlayGameStart -= PlaySound;
                break;
            case SoundCategory.NOM:
                AudioEvents.OnPlayNom -= PlaySound;
                break;
            case SoundCategory.SPIT:
                AudioEvents.OnPlaySpit -= PlaySound;
                break;
            case SoundCategory.POWERUP:
                AudioEvents.OnPlayPowerUp -= PlaySound;
                break;
            case SoundCategory._CHEST_OPEN:
                AudioEvents.OnPlayChestOpen -= PlaySound;
                break;
        }
    }
}
