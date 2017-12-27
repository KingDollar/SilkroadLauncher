namespace SilkroadLauncher
{
    using System.Collections.Generic;
    using System.IO;
    static class Global
    {
        public static Launcher MainForm;
        public static string path = Directory.GetCurrentDirectory();
        public static string MediaPassword = "169841";

        public static List<KeyValuePair<string,string>> NoticeDicitonary = new List<KeyValuePair<string, string>>();

        //client relation
        public static string _ClientIP;
        public static int _ClientPort;
        public static int _ClientVer;

    }
}
