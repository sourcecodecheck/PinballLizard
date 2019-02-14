using UnityEngine;

public class LoginSettings : MonoBehaviour
{
    public PlayFabSharedSettings PlayFabSettings;
    public string TitleId;
    public string ProductionEnvironmentUrl;
    public GameObject TitleIdPopUp;
    
    void Awake()
    {
        LogOnEvents.OnTryLogin += TryLogin;
        LogOnEvents.OnLoginFailure += LoginFailed;
        LogOnEvents.OnLoginSuccess += LoginSucceeded;
        PlayFabSettings.ProductionEnvironmentUrl = ProductionEnvironmentUrl;
        if (string.IsNullOrEmpty(TitleId) == false)
        {
            PlayFabSettings.TitleId = TitleId;
            LoginHelper.Login();
        }
        else if (PlayerPrefs.HasKey(PlayerPrefsKeys.PlayFabTitleId))
        {
            PlayFabSettings.TitleId = PlayerPrefs.GetString(PlayerPrefsKeys.PlayFabTitleId);
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
        PlayFabSettings.TitleId = titleId;
        PlayerPrefs.SetString(PlayerPrefsKeys.PlayFabTitleId, titleId);
        PlayerPrefs.Save();
        LoginHelper.Login();
    }
    void LoginSucceeded()
    {
        MenuEvents.SendChangeMenu(MenuEvents.Menus.MAIN);
    }
    void LoginFailed()
    {
        TitleIdPopUp.SetActive(true);
    }

    private void OnDestroy()
    {
        LogOnEvents.OnTryLogin -= TryLogin;
        LogOnEvents.OnLoginFailure -= LoginFailed;
        LogOnEvents.OnLoginSuccess -= LoginSucceeded;
    }
}
