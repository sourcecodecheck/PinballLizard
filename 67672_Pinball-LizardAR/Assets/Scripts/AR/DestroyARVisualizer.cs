using UnityEngine;

public class DestroyARVisualizer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GamePlayEvents.OnDestroyARVisualizers += DestroySelf;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnDestroyARVisualizers -= DestroySelf;
    }
}
