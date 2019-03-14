using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SendSessionEnd : MonoBehaviour
{
    public int sceneToChangeTo;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoSessionEnd);
    }
    
    void Update()
    {
    }

    private void DoSessionEnd()
    {
        TrackingEvents.SendBuildSessionEndStep2(new CitySessionEnd() { ExitType = "quit" }, EventNames.SessionEnd);
    }
}
