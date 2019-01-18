public static class MenuTransitionEvents
{
    public enum Menus {TITLE = 0, MAIN, PLAYERINFO, STORE, SETTINGS, AR, DAILY_CHALLENGE }
    public delegate void ChangeMenu(Menus menu);
    public static event ChangeMenu OnChangeMenu;
    public static void SendChangeMenu(Menus menu)
    {
        OnChangeMenu?.Invoke(menu);
    }

    public delegate void LoadPlayerInfoScreen();
    public static event LoadPlayerInfoScreen OnLoadPlayerInfoScreen;
    public static void SendLoadPlayerInfoScreen()
    {
        OnLoadPlayerInfoScreen?.Invoke();
    }

    public delegate void LoadSettingsScreen();
    public static event LoadSettingsScreen OnLoadSettingsScreen;
    public static void SendLoadSettingsScreen()
    {
        OnLoadSettingsScreen?.Invoke();
    }
}
