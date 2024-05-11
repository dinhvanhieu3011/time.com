using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BMBSOFT.GIS.CORE.GeoServer
{
    public class CUrlHelper
    {
        public static string Tb2WMS(string geoConStr
            , string pgConStr
            , string path2Curl
            , string tb
            , string workspace
            , string storeName
            , string styleName
            , bool isPoligon)
        {

            GSInfo gs = new GSInfo(geoConStr);
            ConnectInfo ci = new ConnectInfo(pgConStr);
            var array = tb.Split('.');
            //var storeName = ci.Database + "_" + array[1];
            //var storeFilename = array[1] + "_Store_Name" + ".xml";
            var layerName = array[1];
            //var a1 = "echo ^<dataStore^>^<name^>" + storeName + "^</name^>^<type^>PostGIS^</type^>^<enabled^>true^</enabled^>^<workspace^>^<name^>" + workspace + "^</name^>^</workspace^>^<connectionParameters^>^<entry key=\"port\"^>" + ci.Port + "^</entry^>^<entry key=\"user\"^>" + ci.User + "^</entry^>^<entry key=\"passwd\"^>" + ci.Pass + "^</entry^>^<entry key=\"dbtype\"^>postgis^</entry^>^<entry key=\"host\"^>" + ci.Host + "^</entry^>^<entry key=\"database\"^>" + ci.Database + "^</entry^>^<entry key=\"schema\"^>" + array[0] + "^</entry^>^</connectionParameters^>^<__default^>false^</__default^>^</dataStore^>>" + storeFilename + Environment.NewLine;
            //var a1 = "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST -T " + path2Curl + "\\" + storeFilename + " -H \"Content-type: text/xml\" " + gs.domain + "/workspaces/" + workspace + "/datastores" + Environment.NewLine;
            var a1 = "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST -H \"Content-type: text/xml\" -d \"<featureType><name>" + layerName + "</name></featureType>\" " + gs.domain + "/workspaces/" + workspace + "/datastores/" + storeName + "/featuretypes" + Environment.NewLine;

            if (isPoligon)
                a1 += "curl -v -k -u " + gs.username + ":" + gs.password + " -X PUT -H \"Content-type: text/xml\" -d \"<layer><defaultStyle><name>" + styleName + "</name></defaultStyle></layer>\" " + gs.domain + "/layers/" + layerName;

            string filename = "B3_Publish_Layer_" + Guid.NewGuid().ToString() + ".bat";
            File.WriteAllText(path2Curl + filename, a1);

            return filename;
        }

        public static string Reload(string geoConStr)
        {
            GSInfo gs = new GSInfo(geoConStr);
            var curl = "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST -H \"Content-type: text/plain\" " + gs.domain + "/reload";
            return "Ok";
        }

        public static string DeleteStyle(string geoConStr, string workspace, string sldname)
        {
            GSInfo gs = new GSInfo(geoConStr);
            var curl = "curl -v -k -u " + gs.username + ":" + gs.password + " -X DELETE " + gs.domain + "workspaces/" + workspace + "/styles/" + sldname + "?purge=true";

            return "Ok";
        }

        public static string CreateSld(string geoConStr, string path2Curl, string sldName, string zipFileSld, string workspace)
        {
            GSInfo gs = new GSInfo(geoConStr);
            var a = "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST -H \"Content-type: text/xml\" -d \"<style><name>" + sldName + "</name><filename>" + sldName + ".sld</filename></style>\" " + gs.domain + "/workspaces/" + workspace + "/styles" + Environment.NewLine;
            a += "curl -v -k -u " + gs.username + ":" + gs.password + " -X PUT -H \"Content-type: application/vnd.ogc.sld+xml\" -d @" + zipFileSld + " " + gs.domain + "/workspaces/" + workspace + "/styles/" + sldName;

            string filename = "B2_Create_SLD_" + Guid.NewGuid().ToString() + ".bat";
            File.WriteAllText(path2Curl + filename, a);

            return filename;
        }


        public static string ImportWsTif(string geoConStr, string importPath)
        {
            GSInfo gs = new GSInfo(geoConStr);
            var curl = "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST -H \"Content-type: application/json\" -d @" + importPath + " " + gs.domain + "/imports";
            return "Ok";
        }

        public static string ImportTif(string geoConStr, string importPath)
        {
            GSInfo gs = new GSInfo(geoConStr);
            var curl = "curl -v -k -u " + gs.username + ":" + gs.password + " -F name=test -F filedata=@" + importPath + " " + gs.domain + "/imports/0/tasks";
            curl += "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST " + gs.domain + "/imports/0";
            return "Ok";
        }

        public static string ImportTransfornationTif(string geoConStr, string importPath)
        {
            GSInfo gs = new GSInfo(geoConStr);
            var curl = "curl -v -k -u " + gs.username + ":" + gs.password + " -X POST -H \"Content-type: application/json\" -d @" + importPath + " " + gs.domain + "/imports/0/tasks/0/transforms";
            return "Ok";
        }
    }
}
