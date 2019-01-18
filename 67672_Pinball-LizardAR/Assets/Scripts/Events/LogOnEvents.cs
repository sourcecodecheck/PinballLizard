public static class LogOnEvents
{ 
    public delegate void TryLogin(string titleId);
    public static event TryLogin OnTryLogin;
    public static void SendTryLogin(string titleId)
    {
        OnTryLogin?.Invoke(titleId);
    }

    public delegate void LoginSuccess();
    public static event LoginSuccess OnLoginSuccess;
    public static void SendLoginSuccess()
    {
        OnLoginSuccess?.Invoke();
    }

    public delegate void LoginFailure();
    public static event LoginFailure OnLoginFailure;
    public static void SendLoginFailure()
    {
        OnLoginFailure?.Invoke();
    }
}
