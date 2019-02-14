public class AudioEvents
{
    public delegate void PlayBuildingCollapse();
    public static event PlayBuildingCollapse OnPlayBuildingCollapse;
    public static void SendPlayBuildingCollapse()
    {
        OnPlayBuildingCollapse?.Invoke();
    }

    public delegate void PlayBugSmack();
    public static event PlayBugSmack OnPlayBugSmack;
    public static void SendPlayBugSmack()
    {
        OnPlayBugSmack?.Invoke();
    }

    public delegate void PlayMenuBoop();
    public static event PlayMenuBoop OnPlayMenuBoop;
    public static void SendPlayMenuBoop()
    {
        OnPlayMenuBoop?.Invoke();
    }
}

