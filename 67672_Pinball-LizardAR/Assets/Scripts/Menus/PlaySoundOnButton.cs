using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnButton : MonoBehaviour
{
    public SoundPlayer.SoundCategory Category;
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
            case SoundPlayer.SoundCategory.BUILDING:
                AudioEvents.SendPlayBuildingCollapse();
                break;
            case SoundPlayer.SoundCategory.SMACK:
                AudioEvents.SendPlayBugSmack();
                break;
            case SoundPlayer.SoundCategory.BOOP:
                AudioEvents.SendPlayMenuBoop();
                break;
        }
    }
}
