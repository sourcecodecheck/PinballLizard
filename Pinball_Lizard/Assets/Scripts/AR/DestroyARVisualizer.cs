using UnityEngine;

public class DestroyARVisualizer : MonoBehaviour
{
    void Start()
    {
        GamePlayEvents.OnDestroyARVisualizers += DestroySelf;
    }

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
