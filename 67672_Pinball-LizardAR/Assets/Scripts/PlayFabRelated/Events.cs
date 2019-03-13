using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.EventsModels;
using System;
using Microsoft.AppCenter.Unity.Crashes;


public class Events : MonoBehaviour
{
    private List<EventContents> eventQueue;

    // Use this for initialization
    void Awake()
    {
        eventQueue = new List<EventContents>();
        TrackingEvents.OnPlayFabTitleEvent += SendTitleEvent;
        TrackingEvents.OnPlayFabPlayerEvent += SendPlayerEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendTitleEvent(Dictionary<string, object> eventBody, string eventTitle)
    {

        PlayFabClientAPI.WriteTitleEvent(
            new WriteTitleEventRequest()
            {
                Body = eventBody,
                EventName = eventTitle,
                Timestamp = DateTime.Now
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
            }
       );
    }

    public void SendPlayerEvent(Dictionary<string, object> eventBody, string eventTitle)
    {
        PlayFabClientAPI.WritePlayerEvent(
            new WriteClientPlayerEventRequest()
            {
                Body = eventBody,
                EventName = eventTitle,
                Timestamp = DateTime.Now
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
            }
       );
    }

    void SendPlayStreamEvents(List<EventContents> events)
    {
        PlayFabEventsAPI.WriteEvents(new PlayFab.EventsModels.WriteEventsRequest()
        {
            Events = events
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
        TrackingEvents.OnPlayFabTitleEvent -= SendTitleEvent;
        TrackingEvents.OnPlayFabPlayerEvent -= SendPlayerEvent;
    }
}
