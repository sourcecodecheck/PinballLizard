using UnityEngine;
using UnityEngine.UI;

public class EnableObject : MonoBehaviour
{
    public GameObject ObjectToEnable;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoEnable);
    }
    
    void Update()
    {
    }

    private void DoEnable()
    {
        TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "RevealButton" }, EventNames.UiAction);
        ObjectToEnable.SetActive(true);
    }
}
