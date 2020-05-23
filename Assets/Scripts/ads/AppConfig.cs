public static class AppConfig
{
    
#if UNITY_EDITOR
    public static string adUnitId = "unused";
#elif UNITY_ANDROID
    //public static string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test Ad
    public static string adUnitId = "ca-app-pub-8938056874619269/2918817667"; // Real Ad
#elif UNITY_IPHONE
    public static string adUnitId = "unexpected_platform";
#else
    public static string adUnitId = "unexpected_platform";
#endif

    //public static string ironSourceAppKey = "c507665d"; // Ciberman
    public static string ironSourceAppKey = "c51b2d1d"; // AppSoluteGames

    //public static string tenjinAppKey = "HMFVJUPHW378PYXCFSCU9ZJ1YTXKDGOR"; // Ciberman
    public static string tenjinAppKey = "A1VXEYGAFV7V1UWSEPEVZPWEXXD6YY8M"; // AppSoluteGames

    public static string[] testDevicesIds = new string[] {
        "6AFA163589E44B52902FF87313B07633", // My own cellphone (Ciberman)
    };

}