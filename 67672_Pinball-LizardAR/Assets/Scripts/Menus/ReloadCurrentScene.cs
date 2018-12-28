using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadCurrentScene : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Reload);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
