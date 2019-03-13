using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class iOSDeviceIdLogin
{
    public static void LoginPlayfabWithDeviceID()
    {
        PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            DeviceModel = SystemInfo.deviceName,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true,
            TitleId = PlayFabSettings.TitleId
        },
       (response) =>
       {
           PlayerPrefs.SetString(PlayerPrefsKeys.SessionTicket, response.SessionTicket);
           PlayerPrefs.SetString(PlayerPrefsKeys.PlayFabId, response.PlayFabId);
           PlayerPrefs.SetString(PlayerPrefsKeys.PlayerEntityId, response.EntityToken.Entity.Id);
           PlayerPrefs.Save();
           LogOnEvents.SendLoginSuccess();
       },
       (error) =>
       {
           LogOnEvents.SendLoginFailure();
       });
    }
}
