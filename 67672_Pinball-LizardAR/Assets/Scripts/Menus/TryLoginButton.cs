using UnityEngine;
using UnityEngine.UI;

public class TryLoginButton: MonoBehaviour
{
    public Button OnPress;
    public Text Input;
    public Text TryAgain;
    
    void Start()
    {
        LogOnEvents.OnLoginFailure += LoginFailed;
        OnPress.onClick.AddListener(OnClick);
    }

    
    void Update()
    {

    }
    void OnClick()
    {
        LogOnEvents.SendTryLogin(Input.text);
    }

    void LoginFailed()
    {
        TryAgain.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        LogOnEvents.OnLoginFailure -= LoginFailed;
    }
}
