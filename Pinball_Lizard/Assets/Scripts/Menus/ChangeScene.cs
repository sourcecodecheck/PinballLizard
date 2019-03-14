using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public int sceneToChangeTo;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoSceneChange);
    }
    
    void Update()
    {
    }

    private void DoSceneChange()
    {
        TrackingEvents.SendBuildPlayerEvent(new PlayerUIAction() { UIAction = "LoadSceneButton" }, EventNames.UiAction);
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
