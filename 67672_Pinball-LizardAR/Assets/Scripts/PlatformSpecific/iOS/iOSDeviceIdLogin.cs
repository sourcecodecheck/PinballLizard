using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class iOSDeviceIdLogin
{
	public static void LoginPlayfabWithDeviceID() {
        PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            DeviceModel = SystemInfo.deviceName,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true,
            TitleId = PlayFabSettings.TitleId
        },
       (response) => {
           PlayerPrefs.SetString("sessionticket", response.SessionTicket);
           PlayerPrefs.SetString("playfabid", response.PlayFabId);
           PlayerPrefs.Save();
       },
       (error) => {
           Debug.Log("Whoops");
       });
    }
}
