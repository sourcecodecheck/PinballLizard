using UnityEngine;
using UnityEngine.UI;

public class EnableObject : MonoBehaviour
{

    public GameObject ObjectToEnable;
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoEnable);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void DoEnable()
    {
        ObjectToEnable.SetActive(true);
    }
}
