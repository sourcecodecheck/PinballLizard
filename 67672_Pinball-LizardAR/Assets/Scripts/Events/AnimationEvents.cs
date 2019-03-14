public static class AnimationEvents
{
    //Subscribers:
    //MouthBehavior
    public delegate void MouthEnter();
    public static event MouthEnter OnMouthEnter;
    public static void SendMouthEnter()
    {
            OnMouthEnter?.Invoke();
    }

    //Subscribers:
    //MouthBehavior
    //SpawnEnemies
    public delegate void MouthEntered();
    public static event MouthEntered OnMouthEntered;
    public static void SendMouthEntered()
    {
            OnMouthEntered?.Invoke();
    }

    //Subscribers:
    public delegate void MouthNom();
    public static event MouthNom OnMouthNom;
    public static void SendMouthNom()
    {
            OnMouthNom?.Invoke();
    }

    //Subscribers:
    //MouthBehavior
    public delegate void MouthNommed();
    public static event MouthNommed OnMouthNommed;
    public static void SendMouthNommed()
    {
            OnMouthNommed?.Invoke();
    }

    //Subscribers:
    public delegate void MouthExit();
    public static event MouthExit OnMouthExit;
    public static void SendMouthExit()
    {
            OnMouthExit?.Invoke();
    }

    //Subscribers:
    //MouthBehavior
    public delegate void MouthExited();
    public static event MouthExited OnMouthExited;
    public static void SendMouthExited()
    {
            OnMouthExited?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void HandsEnter();
    public static event HandsEnter OnHandsEnter;
    public static void SendHandsEnter()
    {
            OnHandsEnter?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void HandsEntered();
    public static event HandsEntered OnHandsEntered;
    public static void SendHandsEntered()
    { 
            OnHandsEntered?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void LeftHandSwipe();
    public static event LeftHandSwipe OnLeftHandSwipe;
    public static void SendLeftHandSwipe()
    {
            OnLeftHandSwipe?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void LeftHandSwiped();
    public static event LeftHandSwiped OnLeftHandSwiped;
    public static void SendLeftHandSwiped()
    {
            OnLeftHandSwiped?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void HandsExit();
    public static event HandsExit OnHandsExit;
    public static void SendHandsExit()
    {
            OnHandsExit?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    //MainGameManager
    public delegate void HandsExited();
    public static event HandsExited OnHandsExited;
    public static void SendHandsExited()
    {
            OnHandsExited?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void RightHandSwipe();
    public static event RightHandSwipe OnRightHandSwipe;
    public static void SendRightHandSwipe()
    {
            OnRightHandSwipe?.Invoke();
    }

    //Subscribers:
    //ArmBehavior
    public delegate void RightHandSwiped();
    public static event RightHandSwiped OnRightHandSwiped;
    public static void SendRightHandSwiped()
    {
            OnRightHandSwiped?.Invoke();
    }

    //Subscribers:
    public delegate void BannerEnter();
    public static event BannerEnter OnBannerEnter;
    public static void SendBannerEnter()
    {
        OnBannerEnter?.Invoke();
    }

    //Subscribers:
    public delegate void BannerExited();
    public static event BannerExited OnBannerExited;
    public static void SendBannerExited()
    {
        OnBannerExited?.Invoke();
    }

    ////Subscribers:
    ///MainGameUIManager
    public delegate void MissEnter();
    public static event MissEnter OnMissEnter;
    public static void SendMissEnter()
    {
        OnMissEnter?.Invoke();
    }

    //Subscribers:
    //MainGameUIManager
    public delegate void MissExited();
    public static event BannerExited OnMissExited;
    public static void SendMissExited()
    {
        OnMissExited?.Invoke();
    }

    //Subscribers:
    //ChestDisabler
    public delegate void ChestOpened();
    public static event ChestOpened OnChestOpened;
    public static void SendChestOpened()
    {
        OnChestOpened?.Invoke();
    }

    //Subscribers:
    //ScoreDisplay
    public delegate void DoublePointsExit();
    public static event DoublePointsExit OnDoublePointsExit;
    public static void SendDoublePointsExit()
    {
        OnDoublePointsExit?.Invoke();
    }
}
