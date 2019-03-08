class ShowMessageWindowHelper
{
    public static void ShowMessage(string message)
    {
#if UNITY_ANDROID
        AndroidHelpers.ShowAndroidToastMessage(message);
#endif
#if UNITY_IOS
        //Turns out there is no iOS toast message!
#endif
    }
}
