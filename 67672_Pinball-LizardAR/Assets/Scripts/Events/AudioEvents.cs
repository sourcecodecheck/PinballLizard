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

    public delegate void PlayMenuBoop1();
    public static event PlayMenuBoop1 OnPlayMenuBoop1;
    public static void SendPlayMenuBoop1()
    {
        OnPlayMenuBoop1?.Invoke();
    }

    public delegate void PlayMenuBoop2();
    public static event PlayMenuBoop2 OnPlayMenuBoop2;
    public static void SendPlayMenuBoop2()
    {
        OnPlayMenuBoop2?.Invoke();
    }


    public delegate void PlayMenuBoop3();
    public static event PlayMenuBoop3 OnPlayMenuBoop3;
    public static void SendPlayMenuBoop3()
    {
        OnPlayMenuBoop3?.Invoke();
    }

    public delegate void PlayMenuBoop4();
    public static event PlayMenuBoop4 OnPlayMenuBoop4;
    public static void SendPlayMenuBoop4()
    {
        OnPlayMenuBoop4?.Invoke();
    }

    public delegate void PlayItemGet();
    public static event PlayItemGet OnPlayItemGet;
    public static void SendPlayItemGet()
    {
        OnPlayItemGet?.Invoke();
    }

    public delegate void PlayNom();
    public static event PlayNom OnPlayNom;
    public static void SendPlayNom()
    {
        OnPlayNom?.Invoke();
    }

    public delegate void PlayPowerUp();
    public static event PlayPowerUp OnPlayPowerUp;
    public static void SendPlayPowerUp()
    {
        OnPlayPowerUp?.Invoke();
    }

    public delegate void PlaySpit();
    public static event PlaySpit OnPlaySpit;
    public static void SendPlaySpit()
    {
        OnPlaySpit?.Invoke();
    }

    public delegate void PlayGameStart();
    public static event PlayGameStart OnPlayGameStart;
    public static void SendPlayGameStart()
    {
        OnPlayGameStart?.Invoke();
    }

    public delegate void PlayUp();
    public static event PlayUp OnPlayUp;
    public static void SendPlayUp()
    {
        OnPlayUp?.Invoke();
    }

    public delegate void PlayDown();
    public static event PlayDown OnPlayDown;
    public static void SendPlayDown()
    {
        OnPlayDown?.Invoke();
    }
}

