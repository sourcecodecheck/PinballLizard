using UnityEngine;
using UnityEngine.UI;
using System;

public class EventTimer : MonoBehaviour
{
    public Text Hours;
    public Text Minutes;

    private DateTime endTime;

    void Start()
    {
        endTime = DateTime.Today.AddDays(1);
        MenuEvents.OnChallengeModeEndRetrieved += UpdateEndTime;
    }
    
    void Update()
    {
        TimeSpan timeRemaining = endTime - DateTime.Now;
        Hours.text = timeRemaining.Hours.ToString() + "hrs";
        Minutes.text = timeRemaining.Minutes.ToString() + "min";
    }

    void UpdateEndTime(DateTime newEndTime)
    {
        endTime = newEndTime;
    }
    private void OnDestroy()
    {
        MenuEvents.OnChallengeModeEndRetrieved -= UpdateEndTime;
    }

}
