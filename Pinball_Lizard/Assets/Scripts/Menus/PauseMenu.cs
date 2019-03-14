using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Start()
    {
        GamePlayEvents.OnPause += OnUnPause;
    }
    
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
