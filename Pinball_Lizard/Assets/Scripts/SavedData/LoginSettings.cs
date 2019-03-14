using UnityEngine;

public class LoginSettings : MonoBehaviour
{
    public PlayFabSharedSettings PlayFabSettings;
    public string TitleId;
    public string ProductionEnvironmentUrl;
    public float LoginTimeout;
    public GameObject TitleIdPopUp;
    public GameObject TimeoutMessage;

    private bool haveLoginResponse;
    
    void Awake()
    {
        haveLoginResponse = false;
        LogOnEvents.OnTryLogin += TryLogin;
        LogOnEvents.OnLoginFailure += LoginFailed;
        LogOnEvents.OnLoginSuccess += LoginSucceeded;
        PlayFabSettings.ProductionEnvironmentUrl = ProductionEnvironmentUrl;
        if (string.IsNullOrEmpty(TitleId) == false)
        {
            PlayFabSettings.TitleId = TitleId;
            Invoke("HandleLoginTimeout", LoginTimeout);
            LoginHelper.Login();
        }
        else if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayFabTitleId))
        {
            PlayFabSettings.TitleId = PlayerPrefs.GetString(PlayerPrefsKeys.PlayFabTitleId);
            Invoke("HandleLoginTimeout", LoginTimeout);
            LoginHelper.Login();
        }
        else
        {
            TitleIdPopUp.SetActive(true);
        }
    }

    
    void Update()
    {
    }

    void TryLogin(string titleId)
    {
        haveLoginResponse = false;
        PlayFabSettings.TitleId = titleId;
        PlayerPrefs.SetString(PlayerPrefsKeys.PlayFabTitleId, titleId);
        PlayerPrefs.Save();
        Invoke("HandleLoginTimeout", LoginTimeout);
        LoginHelper.Login();
    }
    void LoginSucceeded()
    {
        MenuEvents.SendChangeMenu(MenuEvents.Menus.MAIN);
        haveLoginResponse = true;
    }
    void LoginFailed()
    {
        TitleIdPopUp.SetActive(true);
        haveLoginResponse = false;
    }
    void HandleLoginTimeout()
    {
        if(haveLoginResponse == false)
        {
            TimeoutMessage.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        LogOnEvents.OnTryLogin -= TryLogin;
        LogOnEvents.OnLoginFailure -= LoginFailed;
        LogOnEvents.OnLoginSuccess -= LoginSucceeded;
    }
}
