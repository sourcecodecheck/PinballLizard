using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GamePlayEvents.OnPause += OnUnPause;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnUnPause(bool loadPauseMenu)
    {
        if (loadPauseMenu == true)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GamePlayEvents.OnPause -= OnUnPause;
    }
}
