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

}
