using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMessage : MonoBehaviour
{
    public Text MessageText;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMessage(string message)
    {
        MessageText.text = message;
    }
}
