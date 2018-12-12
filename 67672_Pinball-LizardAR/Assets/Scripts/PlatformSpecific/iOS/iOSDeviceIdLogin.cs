using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
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
       },
       (error) => {
           Debug.Log("Whoops");
       });
    }
}
