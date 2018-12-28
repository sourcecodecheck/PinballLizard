using UnityEngine;
using UnityEngine.UI;

public class AppetiteDisplay : MonoBehaviour
{

    public Text AppetiteCurrentText;
    public Text AppetiteMaxText;
    // Use this for initialization
    void Awake()
    {
        GamePlayEvents.OnUpdateAppetite += UpdateAppetite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateAppetite(int current, int max)
    {
        AppetiteCurrentText.text = current.ToString();
        AppetiteMaxText.text = max.ToString();
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnUpdateAppetite -= UpdateAppetite;
    }
}
