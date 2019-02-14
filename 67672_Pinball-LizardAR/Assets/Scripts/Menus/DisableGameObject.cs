using UnityEngine;
using UnityEngine.UI;

public class DisableGameObject : MonoBehaviour
{
    public GameObject ObjectToDisable;
    public Button DisableButton;
    void Start()
    {
        DisableButton.onClick.AddListener(DoDisable);
    }
    
    void Update()
    {
    }

    void DoDisable()
    {
        ObjectToDisable.SetActive(false);
    }
}
