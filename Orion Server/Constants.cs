namespace OrionServer
{
    public static class Constants
    {
        public static readonly string WWWDataFolder = "wwwdata";
        public static readonly string DataFolder = $"{Constants.WWWDataFolder}/data";
        public static readonly string ModulesFolder = $"{Constants.WWWDataFolder}/modules";
        public static readonly string PagesFolder = $"{Constants.WWWDataFolder}/pages";
        public static readonly string ErrorSubmitions = $"{Constants.WWWDataFolder}/errors";
        public static readonly string AuthenticationKeysFile = $"{Constants.WWWDataFolder}/authenticationKeys.json";
        public static readonly string DatabaseFile = $"{Constants.WWWDataFolder}/database.json";
    }
}