public static class AnimationEvents
{
    public delegate void MouthEnter();
    public static event MouthEnter OnMouthEnter;
    public static void SendMouthEnter()
    {
            OnMouthEnter?.Invoke();
    }

    public delegate void MouthEntered();
    public static event MouthEntered OnMouthEntered;
    public static void SendMouthEntered()
    {
            OnMouthEntered?.Invoke();
    }

    public delegate void MouthNom();
    public static event MouthNom OnMouthNom;
    public static void SendMouthNom()
    {
            OnMouthNom?.Invoke();
    }

    public delegate void MouthNommed();
    public static event MouthNommed OnMouthNommed;
    public static void SendMouthNommed()
    {
            OnMouthNommed?.Invoke();
    }

    public delegate void MouthExit();
    public static event MouthExit OnMouthExit;
    public static void SendMouthExit()
    {
            OnMouthExit?.Invoke();
    }

    public delegate void MouthExited();
    public static event MouthExited OnMouthExited;
    public static void SendMouthExited()
    {
            OnMouthExited?.Invoke();
    }

    public delegate void HandsEnter();
    public static event HandsEnter OnHandsEnter;
    public static void SendHandsEnter()
    {
            OnHandsEnter?.Invoke();
    }

    public delegate void HandsEntered();
    public static event HandsEntered OnHandsEntered;
    public static void SendHandsEntered()
    { 
            OnHandsEntered?.Invoke();
    }

    public delegate void LeftHandSwipe();
    public static event LeftHandSwipe OnLeftHandSwipe;
    public static void SendLeftHandSwipe()
    {
            OnLeftHandSwipe?.Invoke();
    }

    public delegate void LeftHandSwiped();
    public static event LeftHandSwiped OnLeftHandSwiped;
    public static void SendLeftHandSwiped()
    {
            OnLeftHandSwiped?.Invoke();
    }

    public delegate void HandsExit();
    public static event HandsExit OnHandsExit;
    public static void SendHandsExit()
    {
            OnHandsExit?.Invoke();
    }

    public delegate void HandsExited();
    public static event HandsExited OnHandsExited;
    public static void SendHandsExited()
    {
            OnHandsExited?.Invoke();
    }

    public delegate void RightHandSwipe();
    public static event RightHandSwipe OnRightHandSwipe;
    public static void SendRightHandSwipe()
    {
            OnRightHandSwipe?.Invoke();
    }

    public delegate void RightHandSwiped();
    public static event RightHandSwiped OnRightHandSwiped;
    public static void SendRightHandSwiped()
    {
            OnRightHandSwiped?.Invoke();
    }
}
