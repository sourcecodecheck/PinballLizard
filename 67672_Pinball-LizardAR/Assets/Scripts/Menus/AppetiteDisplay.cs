using UnityEngine;
using UnityEngine.UI;

public class AppetiteDisplay : MonoBehaviour
{
    public Text AppetiteCurrentText;
    public Text AppetiteMaxText;
    
    void Awake()
    {
        GamePlayEvents.OnUpdateAppetite += UpdateAppetite;
    }

    
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
