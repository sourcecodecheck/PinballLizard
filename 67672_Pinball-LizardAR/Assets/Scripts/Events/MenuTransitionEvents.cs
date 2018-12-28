public static class MenuTransitionEvents
{
    public enum Menus {TITLE = 0, MAIN, PLAYERINFO, STORE, SETTINGS, AR, DAILY_CHALLENGE }
    public delegate void ChangeMenu(Menus menu);
    public static event ChangeMenu OnChangeMenu;
    public static void SendChangeMenu(Menus menu)
    {
        OnChangeMenu(menu);
    }

    public delegate void LoadPlayerInfoScreen();
    public static event LoadPlayerInfoScreen OnLoadPlayerInfoScreen;
    public static void SendLoadPlayerInfoScreen()
    {
        OnLoadPlayerInfoScreen();
    }
}
