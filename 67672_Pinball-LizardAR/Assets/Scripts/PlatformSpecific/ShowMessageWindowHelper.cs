using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ShowMessageWindowHelper
{
#if UNITY_ANDROID
    public static void ShowMessage(string message)
    {
        AndroidHelpers.ShowAndroidToastMessage(message);
    }
#endif
#if UNITY_IOS
        public static void ShowMessage(string message)
        {
            //Write iOS code
        }
#endif
}
