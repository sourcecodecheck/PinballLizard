using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class AnimationEvents
{
    public delegate void MouthEnter();
    public static event MouthEnter OnMouthEnter;
    public static void SendMouthEnter()
    {
        OnMouthEnter();
    }

    public delegate void MouthEntered();
    public static event MouthEntered OnMouthEntered;
    public static void SendMouthEntered()
    {
        OnMouthEntered();
    }

    public delegate void MouthNom();
    public static event MouthNom OnMouthNom;
    public static void SendMouthNom()
    {
        OnMouthNom();
    }

    public delegate void MouthNommed();
    public static event MouthNommed OnMouthNommed;
    public static void SendMouthNommed()
    {
        OnMouthNommed();
    }

    public delegate void MouthExit();
    public static event MouthExit OnMouthExit;
    public static void SendMouthExit()
    {
        OnMouthExit();
    }

    public delegate void MouthExited();
    public static event MouthExited OnMouthExited;
    public static void SendMouthExited()
    {
        OnMouthExited();
    }

    public delegate void HandsEnter();
    public static event HandsEnter OnHandsEnter;
    public static void SendHandsEnter()
    {
        OnHandsEnter();
    }

    public delegate void HandsEntered();
    public static event HandsEntered OnHandsEntered;
    public static void SendHandsEntered()
    {
        OnHandsEntered();
    }

    public delegate void LeftHandSwipe();
    public static event LeftHandSwipe OnLeftHandSwipe;
    public static void SendLeftHandSwipe()
    {
        OnLeftHandSwipe();
    }

    public delegate void LeftHandSwiped();
    public static event LeftHandSwiped OnLeftHandSwiped;
    public static void SendLeftHandSwiped()
    {
        OnLeftHandSwiped();
    }

    public delegate void HandsExit();
    public static event HandsExit OnHandsExit;
    public static void SendHandsExit()
    {
        OnHandsExit();
    }

    public delegate void HandsExited();
    public static event HandsExited OnHandsExited;
    public static void SendHandsExited()
    {
        OnHandsExited();
    }

    public delegate void RightHandSwipe();
    public static event RightHandSwipe OnRightHandSwipe;
    public static void SendRightHandSwipe()
    {
        OnRightHandSwipe();
    }

    public delegate void RightHandSwiped();
    public static event RightHandSwiped OnRightHandSwiped;
    public static void SendRightHandSwiped()
    {
        OnRightHandSwiped();
    }
}
