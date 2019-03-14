public static class GamePlayEvents
{
    //Subscribers:
    //ShotBehavior
    public delegate void TryVolley();
    public static event TryVolley OnTryVolley;
    public static void SendTryVolley()
    {
        OnTryVolley?.Invoke();
    }

    //Subscribers:
    //MouthBehavior
    public delegate void ConfirmVolley();
    public static event ConfirmVolley OnConfirmVolley;
    public static void SendConfirmVolley()
    {
        OnConfirmVolley?.Invoke();
    }

    //Subscribers:
    //Pausable
    //PauseMenu
    public delegate void Pause(bool loadPauseMenu);
    public static event Pause OnPause;
    public static void SendPause(bool loadPauseMenu)
    {
        OnPause?.Invoke(loadPauseMenu);
    }

    //Subscribers:
    //MainGameUIManager
    public delegate void LoadPauseMenu();
    public static event LoadPauseMenu OnLoadPauseMenu;
    public static void SendLoadPauseMenu()
    {
        OnLoadPauseMenu?.Invoke();
    }

    //Subscribers:
    //MouthBehavior
    public delegate void ShotDestroyed();
    public static event ShotDestroyed OnShotDestroyed;
    public static void SendShotDestroyed()
    {
        OnShotDestroyed?.Invoke();
    }

    //Subscribers:
    //City
    //Building
    //MainGameManager
    public delegate void BombDetonated(string damageType);
    public static event BombDetonated OnBombDetonated;
    public static void SendBombDetonated(string damageType = "bomb")
    {
        OnBombDetonated?.Invoke(damageType);
    }

    //Subscribers:
    //MainGameManger
    //ScoreDisplay
    public delegate void FeastStart();
    public static event FeastStart OnFeastStart;
    public static void SendFeastStart()
    {
        OnFeastStart?.Invoke();
    }

    //Subscribers:
    //MainGameManager
    //ScoreDisplay
    //PowerUpButton
    public delegate void FeastEnd();
    public static event FeastEnd OnFeastEnd;
    public static void SendFeastEnd()
    {
        OnFeastEnd?.Invoke();
    }

    //Subscribers:
    //MouthBehavior
    public delegate void SpicyReady();
    public static event SpicyReady OnSpicyReady;
    public static void SendSpicyReady()
    {
        OnSpicyReady?.Invoke();
    }

    //Subscribers:
    //PowerUpButton
    public delegate void SpicyEnd();
    public static event SpicyEnd OnSpicyEnd;
    public static void SendSpicyEnd()
    {
        OnSpicyEnd?.Invoke();
    }

    //Subscribers:
    //Inventory
    //MainGameManager
    public delegate void UsePowerUp(string keyTerm);
    public static event UsePowerUp OnUsePowerUp;
    public static void SendUsePowerUp(string keyTerm)
    {
        OnUsePowerUp?.Invoke(keyTerm);
    }

    //Subscribers:
    public delegate void UpdatePowerUps();
    public static event UpdatePowerUps OnUpdatePowerUps;
    public static void SendUpdatePowerUps()
    {
        OnUpdatePowerUps?.Invoke();
    }

    //Subscribers:
    //AppetiteDisplay
    public delegate void UpdateAppetite(int current, int max);
    public static event UpdateAppetite OnUpdateAppetite;
    public static void SendUpdateAppetite(int current, int max)
    {
        OnUpdateAppetite?.Invoke(current, max);
    }

    //Subscribers:
    //DestroyARVisualizer
    public delegate void DestroyARVisualizers();
    public static event DestroyARVisualizers OnDestroyARVisualizers;
    public static void SendDestroyARVisualizers()
    {
        OnDestroyARVisualizers?.Invoke();
    }

    //Subscribers:
    //EnemyBehavior
    public delegate void TryNom(int instanceId);
    public static event TryNom OnTryNom;
    public static void SendTryNom(int instanceId)
    {
        OnTryNom?.Invoke(instanceId);
    }

    //Subscribers:
    //MouthBehavior
    public delegate void ConfirmNom();
    public static event ConfirmNom OnConfirmNom;
    public static void SendConfirmNom()
    {
        OnConfirmNom?.Invoke();
    }

    //Subscribers:
    public delegate void DeConfirmNom();
    public static event DeConfirmNom OnDeConfirmNom;
    public static void SendDeConfirmNom()
    {
        OnDeConfirmNom?.Invoke();
    }

    //Subscribers:
    //AmmoDisplay
    public delegate void UpdateAmmoCount(int current);
    public static event UpdateAmmoCount OnUpdateAmmoCount;
    public static void SendUpdateAmmoCount(int current)
    {
        OnUpdateAmmoCount?.Invoke(current);
    }
}