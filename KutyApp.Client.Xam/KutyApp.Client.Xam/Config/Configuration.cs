namespace KutyApp.Client.Xam.Config
{
    public static class Configurations
    {
        //public static readonly string ConnectionBase = "https://localhost:44364";
        //public static readonly string ConnectionBase = "http://localhost:8733";
        //public static readonly string ConnectionBase = "http://192.168.10.51:45455";
        public static readonly string ConnectionBase = "https://iqtato.synology.me";
        public static string ApiToken { get; set; } = string.Empty;
        public static int ApiVerison = 7;
    }
}
