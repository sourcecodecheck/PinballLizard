using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{

    public int sceneToChangeTo;
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoSceneChange);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void DoSceneChange()
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
