namespace OrionServer
{
    public static class Constants
    {
        public static string WWWDataFolder = "wwwdata";
        public static string DataFolder = $"{WWWDataFolder}/data";
        public static string ModulesFolder = $"{WWWDataFolder}/modules";
        public static string PagesFolder = $"{WWWDataFolder}/pages";
        public static string AuthenticationKeysFile = $"{Constants.WWWDataFolder}/authenticationKeys.json";
    }
}