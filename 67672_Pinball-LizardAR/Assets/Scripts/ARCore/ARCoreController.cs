﻿using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCoreController : MonoBehaviour
{

    public GameObject MainGameWorld;

    public Camera FirstPersonCamera;

    public GameObject SearchingForPlaneUI;

    private bool gameWorldPlaced = false;
    private bool isQuitting = false;
    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        Session.GetTrackables<DetectedPlane>(m_AllPlanes);
        bool showSearchingUI = true;
        for (int i = 0; i < m_AllPlanes.Count; ++i)
        {
            if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
            {
                showSearchingUI = false;
                break;
            }
        }

        SearchingForPlaneUI.SetActive(showSearchingUI);

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {

            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                GameObject prefab = gameWorldPlaced ? null : MainGameWorld;
                if (gameWorldPlaced == false)
                {
                    gameWorldPlaced = true;
                    Anchor anchor = null;
                    InstantiateOnPlane(prefab, hit, out anchor);
                }
            }
        }
    }


    private GameObject InstantiateOnPlane(GameObject prefab, TrackableHit hit, out Anchor anchor)
    {
        GameObject instantiatedObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

        instantiatedObject.transform.Rotate(0, 180.0f, 0, Space.Self);

        anchor = hit.Trackable.CreateAnchor(hit.Pose);

        instantiatedObject.transform.parent = anchor.transform;
        return instantiatedObject;
    }

    public void UpdateState()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (isQuitting)
        {
            return;
        }

        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            AndroidHelpers.ShowAndroidToastMessage("Camera permission is needed to run this application.");
            isQuitting = true;
            Invoke("DelayQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            AndroidHelpers.ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
            isQuitting = true;
            Invoke("DelayQuit", 0.5f);
        }
    }

    private void DelayQuit()
    {
        Application.Quit();
    }
}