using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnButton : MonoBehaviour
{
    public GlobalSoundPlayer.SoundCategory Category;
    public Button PlayOnPress;
    void Start()
    {
        PlayOnPress.onClick.AddListener(PlaySound);
    }


    void Update()
    {
    }

    void PlaySound()
    {
        switch (Category)
        {
            case GlobalSoundPlayer.SoundCategory.BUILDING:
                AudioEvents.SendPlayBuildingCollapse();
                break;
            case GlobalSoundPlayer.SoundCategory.SMACK:
                AudioEvents.SendPlayBugSmack();
                break;
            case GlobalSoundPlayer.SoundCategory.BOOP1:
                AudioEvents.SendPlayMenuBoop1();
                break;
            case GlobalSoundPlayer.SoundCategory.BOOP2:
                AudioEvents.SendPlayMenuBoop2();
                break;
            case GlobalSoundPlayer.SoundCategory.BOOP3:
                AudioEvents.SendPlayMenuBoop3();
                break;
            case GlobalSoundPlayer.SoundCategory.BOOP4:
                AudioEvents.SendPlayMenuBoop4();
                break;
            case GlobalSoundPlayer.SoundCategory.GETITEM:
                AudioEvents.SendPlayItemGet();
                break;
            case GlobalSoundPlayer.SoundCategory.UP:
                AudioEvents.SendPlayUp();
                break;
            case GlobalSoundPlayer.SoundCategory.DOWN:
                AudioEvents.SendPlayDown();
                break;
            case GlobalSoundPlayer.SoundCategory.STARTGAME:
                AudioEvents.SendPlayGameStart();
                break;
            case GlobalSoundPlayer.SoundCategory.NOM:
                AudioEvents.SendPlayNom();
                break;
            case GlobalSoundPlayer.SoundCategory.SPIT:
                AudioEvents.SendPlaySpit();
                break;
            case GlobalSoundPlayer.SoundCategory.POWERUP:
                AudioEvents.SendPlayPowerUp();
                break;
        }
    }
}
