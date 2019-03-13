using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private float timeAlive;
    void Start()
    {
        GamePlayEvents.OnPause += OnUnPause;
    }
    
    void Update()
    {
        timeAlive += Time.deltaTime;
    }
    void OnUnPause(bool loadPauseMenu)
    {
        if (loadPauseMenu == true)
        {
            TrackingEvents.SendBuildCityEvent(new CitySessionResume() { CityPauseDuration = Mathf.RoundToInt(timeAlive) });
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GamePlayEvents.OnPause -= OnUnPause;
    }
}
