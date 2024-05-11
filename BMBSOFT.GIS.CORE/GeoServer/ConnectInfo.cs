using System;
using System.Collections.Generic;
using System.Text;

namespace BMBSOFT.GIS.CORE.GeoServer
{
    public class ConnectInfo
    {
        public string Host = "localhost";

        public string Port = "5432";

        public string User = "postgres";

        public string Pass = "";

        public string Database = "";

        public ConnectInfo(string str)
        {
            string[] array = str.Split(new string[1]
            {
            ";"
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                string[] b = array[i].Trim().Split(new string[1]
                {
                "="
                }, StringSplitOptions.RemoveEmptyEntries);
                switch (b[0].ToLower())
                {
                    case "server":
                        Host = b[1];
                        break;
                    case "port":
                        Port = b[1];
                        break;
                    case "password":
                        Pass = b[1];
                        break;
                    case "user id":
                        User = b[1];
                        break;
                    case "database":
                        Database = b[1];
                        break;
                }
            }
        }
    }
}
