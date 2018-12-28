using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class AndroidDeviceIdLogin
{
    public static string GetDeviceId()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");

        AndroidJavaClass securitySettings = new AndroidJavaClass("android.provider.Settings$Secure");
        string androidDeviceId = securitySettings.CallStatic<string>("getString", contentResolver, "android_id");
        return androidDeviceId;
    }

    public static string GetDeviceName()
    {
        return SystemInfo.deviceName;
    }

    public  static string GetOperatingSystemVersion()
    {
        return SystemInfo.operatingSystem;
    }

    public static void LoginPlayfabWithDeviceID()
    {
        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest() {
            AndroidDeviceId = GetDeviceId(),
            AndroidDevice = GetDeviceName(),
            OS = GetOperatingSystemVersion(),
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

