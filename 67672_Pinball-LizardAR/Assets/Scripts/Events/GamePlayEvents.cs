using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GamePlayEvents
{
    public delegate void Volley(int enemyId);
    public static event Volley OnVolley;
    public static void SendVolley(int enemyId)
    {
        OnVolley(enemyId);
    }

    public delegate void Pause();
    public static event Pause OnPause;
    public static void SendPause()
    {
        OnPause();
    }

    public delegate void LoadPauseMenu();
    public static event LoadPauseMenu OnLoadPauseMenu;
    public static void SendLoadPauseMenu()
    {
        OnLoadPauseMenu();
    }
}