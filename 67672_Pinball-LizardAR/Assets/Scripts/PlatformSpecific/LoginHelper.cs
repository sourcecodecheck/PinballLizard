using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LoginHelper
{
#if UNITY_ANDROID
    public static void Login()
    {
        AndroidDeviceIdLogin.LoginPlayfabWithDeviceID();
    }
#endif
#if UNITY_IOS
    public static void Login()
    {
        iOSDeviceIdLogin.LoginPlayfabWithDeviceID();
    }
#endif
}
