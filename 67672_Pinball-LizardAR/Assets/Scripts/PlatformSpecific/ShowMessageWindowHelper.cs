class ShowMessageWindowHelper
{
    public static void ShowMessage(string message)
    {
#if UNITY_ANDROID
        AndroidHelpers.ShowAndroidToastMessage(message);
#endif
#if UNITY_IOS
        //Write iOS code
#endif
    }
}
