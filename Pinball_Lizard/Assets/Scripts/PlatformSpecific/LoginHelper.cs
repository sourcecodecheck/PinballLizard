using Microsoft.AppCenter.Unity.Crashes;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

class LoginHelper
{
    public static void Login()
    {
#if UNITY_ANDROID
        AndroidDeviceIdLogin.LoginPlayfabWithDeviceID();
#endif
#if UNITY_IOS
         iOSDeviceIdLogin.LoginPlayfabWithDeviceID();
#endif
    }

    public static void SetUpNewPlayer()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.SessionTicket))
        {
            //initialize statistics to default values
            PlayFabClientAPI.ExecuteCloudScript(
                new ExecuteCloudScriptRequest()
                {
                    FunctionName = "initializePlayer"
                },
                (result) =>
                {
                    StoreEvents.SendReloadStore();
                    if (result.Error != null)
                    {
                        try
                        {
                            throw new Exception(result.Error.Message);
                        }
                        catch (Exception exception)
                        {
                            Crashes.TrackError(exception);
                        }
                    }
                },
                (error) =>
                {
                    Debug.Log(error);
                    try
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                    catch (Exception exception)
                    {
                        Crashes.TrackError(exception);
                    }
                });
        }
    }
}
