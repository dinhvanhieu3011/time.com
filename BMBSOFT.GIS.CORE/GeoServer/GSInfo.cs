using System;
using System.Collections.Generic;
using System.Text;

namespace BMBSOFT.GIS.CORE.GeoServer
{
    public class GSInfo
    {
		public string domain;

		public string username = "admin";

		public string password;


        public GSInfo(string str)
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
                    case "domain":
                        domain = b[1];
                        break;
                    case "username":
                        username = b[1];
                        break;
                    case "password":
                        password = b[1];
                        break;
                }
            }
        }
    }
}
