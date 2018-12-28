public static class GamePlayEvents
{
    public delegate void Volley(int enemyId);
    public static event Volley OnVolley;
    public static void SendVolley(int enemyId)
    {
        OnVolley(enemyId);
    }

    public delegate void Pause(bool loadPauseMenu);
    public static event Pause OnPause;
    public static void SendPause(bool loadPauseMenu)
    {
        OnPause(loadPauseMenu);
    }

    public delegate void LoadPauseMenu();
    public static event LoadPauseMenu OnLoadPauseMenu;
    public static void SendLoadPauseMenu()
    {
        OnLoadPauseMenu();
    }

    public delegate void ShotDestroyed();
    public static event ShotDestroyed OnShotDestroyed;
    public static void SendShotDestroyed()
    {
        OnShotDestroyed();
    }

    public delegate void BombDetonated();
    public static event BombDetonated OnBombDetonated;
    public static void SendBombDetonated()
    {
        OnBombDetonated();
    }

    public delegate void FeastStart();
    public static event FeastStart OnFeastStart;
    public static void SendFeastStart()
    {
        OnFeastStart();
    }

    public delegate void FeastEnd();
    public static event FeastEnd OnFeastEnd;
    public static void SendFeastEnd()
    {
        OnFeastEnd();
    }

    public delegate void SpicyReady();
    public static event SpicyReady OnSpicyReady;
    public static void SendSpicyReady()
    {
        OnSpicyReady();
    }

    public delegate void SpicyEnd();
    public static event SpicyEnd OnSpicyEnd;
    public static void SendSpicyEnd()
    {
        OnSpicyEnd();
    }

    public delegate void UsePowerUp(string keyTerm);
    public static event UsePowerUp OnUsePowerUp;
    public static void SendUsePowerUp(string keyTerm)
    {
        OnUsePowerUp(keyTerm);
    }

    public delegate void UpdatePowerUps();
    public static event UpdatePowerUps OnUpdatePowerUps;
    public static void SendUpdatePowerUps()
    {
        OnUpdatePowerUps();
    }

    public delegate void UpdateAppetite(int current, int max);
    public static event UpdateAppetite OnUpdateAppetite;
    public static void SendUpdateAppetite(int current, int max)
    {
        OnUpdateAppetite(current, max);
    }
}