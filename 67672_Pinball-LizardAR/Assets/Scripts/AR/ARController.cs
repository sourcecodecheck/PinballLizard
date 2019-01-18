using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARController : MonoBehaviour
{
    public ARSession Session;
    public ARSessionOrigin SessionOrigin;
    public ARPlaneManager PlaneManager;
    public ARPointCloudManager PointCloudManager;
    public ARPlaneMeshVisualizer PlaneMeshVisualizer;
    public ARPointCloudMeshVisualizer PointCloudMeshVisualizer;
    public Camera FirstPersonCamera;
    public GameObject MainGameWorld;
    public GameObject SearchingForPlaneUI;
    public Text InstructionText;

    private bool gameWorldPlaced;
    private bool showInstructionUI;
    private GameObject city;
    void Start()
    {
        gameWorldPlaced = false;
        InstructionText.text = "Searching for planes\n gently move your phone left to right.";
        city = null;
        showInstructionUI = true;
    }


    void Update()
    {
        SearchingForPlaneUI.SetActive(showInstructionUI);

        //UpdateState();
        if (PlaneManager.planeCount > 0)
        {
            InstructionText.text = "Tap on the colored area to place the city";
        }
        else
        {
            InstructionText.text = "Searching for planes\n gently move your phone left to right.";
            showInstructionUI = true;
        }

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (SessionOrigin.Raycast(new Vector3(touch.position.x, touch.position.y), hits, TrackableType.PlaneWithinBounds))
        {
            foreach (ARRaycastHit hit in hits)
            {
                if (Vector3.Dot(FirstPersonCamera.transform.position - hit.pose.position,
                        hit.pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the plane");
                }
                else
                {
                    GameObject prefab = gameWorldPlaced ? null : MainGameWorld;
                    if (gameWorldPlaced == false && city == null)
                    {
                        gameWorldPlaced = true;
                        city = InstantiateOnPlane(prefab, hit);
                        showInstructionUI = false;
                        PlaneManager.enabled = false;
                        PointCloudManager.enabled = false;
                        GamePlayEvents.SendDestroyARVisualizers();
                        break;
                    }
                }
            }
        }
    }


    private GameObject InstantiateOnPlane(GameObject prefab, ARRaycastHit hit)
    {
        GameObject instantiatedObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);


        instantiatedObject.transform.Rotate(0, 180.0f, 0, Space.Self);
        return instantiatedObject;
    }

    //public void UpdateState()
    //{
    //    if (Session. != TrackingState.Tracking)
    //    {
    //        const int lostTrackingSleepTimeout = 15;
    //        Screen.sleepTimeout = lostTrackingSleepTimeout;
    //    }
    //    else
    //    {
    //        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    //    }

    //}
}
