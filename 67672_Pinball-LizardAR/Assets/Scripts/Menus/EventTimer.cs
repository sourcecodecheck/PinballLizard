using UnityEngine;
using UnityEngine.UI;
using System;

public class EventTimer : MonoBehaviour
{
    public Text Hours;
    public Text Minutes;

    void Start()
    {
    }

    
    void Update()
    {
        TimeSpan timeRemaining = DateTime.Today.AddDays(1) - DateTime.Now;
        Hours.text = timeRemaining.Hours.ToString() + "hrs";
        Minutes.text = timeRemaining.Minutes.ToString() + "min";
    }

}
