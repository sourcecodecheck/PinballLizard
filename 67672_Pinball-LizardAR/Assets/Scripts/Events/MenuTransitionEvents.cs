using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class MenuTransitionEvents
{
    public enum Menus {TITLE = 0, MAIN, PLAYERINFO, STORE, SETTINGS, AR, DAILY_CHALLENGE }
    public delegate void ChangeMenu(Menus menu);
    public static event ChangeMenu OnChangeMenu;
    public static void SendChangeMenu(Menus menu)
    {
        OnChangeMenu(menu);
    }
}
