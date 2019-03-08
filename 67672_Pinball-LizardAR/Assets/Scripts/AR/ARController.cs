using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine;

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
    public AudioSource ScanningMusic;

    private bool gameWorldPlaced;
    private GameObject city;
    void Start()
    {
        gameWorldPlaced = false;
        city = null;
    }

    void Update()
    {
        if (PlaneManager.planeCount > 0 && gameWorldPlaced == false)
        {
            SearchingForPlaneUI.SetActive(true);
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
                        ScanningMusic.Stop();
                        gameWorldPlaced = true;
                        city = InstantiateOnPlane(prefab, hit);
                        SearchingForPlaneUI.SetActive(false);
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
}
