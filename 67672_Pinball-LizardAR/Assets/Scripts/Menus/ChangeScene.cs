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
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
