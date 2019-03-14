using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayFab;
using PlayFab.EventsModels;
using System;
using Microsoft.AppCenter.Unity.Crashes;


public class Events : MonoBehaviour
{
    public int EventsSentAtOnce;
    public float EventSendRate;
    public bool ClearQueueOnSend;

    private static List<EventContents> eventQueue;

    // Use this for initialization
    void Awake()
    {
        if (eventQueue == null)
        {
            eventQueue = new List<EventContents>();
        }
        TrackingEvents.OnQueueEvent += QueueEvent;
        InvokeRepeating("SendEvents", 1f, EventSendRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SendEvents()
    {
        if (eventQueue != null && eventQueue.Count > 0)
        {
            SendPlayStreamEvents(eventQueue);
            if (ClearQueueOnSend)
            {
                eventQueue.Clear();
            }
            else
            {
                eventQueue.RemoveRange(0, EventsSentAtOnce);
            }
        }
    }
    void QueueEvent(IPlayerEvent playerEvent, string name)
    {
        EventContents tobeQueued = new EventContents();
        tobeQueued.Payload = playerEvent;
        tobeQueued.OriginalTimestamp = DateTime.Now.ToUniversalTime();
        tobeQueued.EventNamespace = "com.playfab.events.pinballlizard";
        tobeQueued.Entity = new PlayFab.EventsModels.EntityKey()
        {
            Id = PlayerPrefs.GetString(PlayerPrefsKeys.PlayerEntityId),
            Type = "title_player_account"
        };
        tobeQueued.Name = name;
        eventQueue.Add(tobeQueued);
    }

    void SendPlayStreamEvents(List<EventContents> events)
    {
        PlayFabEventsAPI.WriteEvents(new PlayFab.EventsModels.WriteEventsRequest()
        {
            Events = events.Take(5).ToList()
        },
        (result) =>
        {
        },
        (error) =>
        {
            Debug.Log(error.ErrorMessage);
            try
            {
                throw new Exception(error.ErrorMessage);
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        });
    }

    private void OnDestroy()
    {
        TrackingEvents.OnQueueEvent -= QueueEvent;
    }
}
