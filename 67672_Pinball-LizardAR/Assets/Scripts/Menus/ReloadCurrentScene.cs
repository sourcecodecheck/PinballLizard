using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadCurrentScene : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Reload);
    }
    
    void Update()
    {
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
