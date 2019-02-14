using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
	public Text AmmoCurrentText;

    void Awake()
    {
        GamePlayEvents.OnUpdateAmmoCount += UpdateAmmo;
    }


    void Update()
    {

    }

    void UpdateAmmo(int current)
    {
        AmmoCurrentText.text = current.ToString();
    }

    private void OnDestroy()
    {
        GamePlayEvents.OnUpdateAmmoCount -= UpdateAmmo;
    }
}
