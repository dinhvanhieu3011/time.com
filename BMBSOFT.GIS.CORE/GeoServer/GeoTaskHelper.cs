using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static BMBSOFT.GIS.CORE.Helper.Constant;

namespace BMBSOFT.GIS.CORE.GeoServer
{
    public class GeoTaskHelper
    {
        public static string CreateBatchfileImportSHP(string saveFolder, string sConnection,
            string pathSHP,
            string tbname,
            string schemaTable,
            string planningCode,
            int idMap,
            string SRID = "4326",
            string pgVersion = "12"
            )
        {
            ConnectInfo ci = new ConnectInfo(sConnection);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("set root=C:\\Program Files\\PostgreSQL\\{0}\\bin", pgVersion);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("CD /D %root%");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGHOST={0}", ci.Host);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPASSWORD={0}", ci.Pass);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPORT={0}", ci.Port);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("shp2pgsql -I -s {0} {1} {2} | psql -U {3} -h {4} -p {5} -d {6}", SRID, pathSHP, schemaTable, ci.User, ci.Host, ci.Port, ci.Database);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"SELECT boundaries.merge_layer('{4}','{5}','{6}','{7}')\"", ci.Database, ci.User, ci.Host, ci.Port,
                schemaTable, tbname, planningCode, idMap);
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"alter table {4} add column if not exists style character varying(254)\"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"alter table {4} add column if not exists name character varying(254)\"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            stringBuilder.AppendLine();
            string filename = "Boundaries_Import_SHP_" + Guid.NewGuid().ToString() + ".bat";

            File.WriteAllText(saveFolder + "\\" + filename, stringBuilder.ToString());
            return filename;
        }
        public static string CreateBatchfileImportReflect(string saveFolder, string sConnection,
    string pathSHP,
     List<long> documents,
    string schemaTable,
    string planningCode,
    int idMap,
    string SRID = "4326",
    string pgVersion = "12"
    )
        {
            ConnectInfo ci = new ConnectInfo(sConnection);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("set root=C:\\Program Files\\PostgreSQL\\{0}\\bin", pgVersion);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("CD /D %root%");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGHOST={0}", ci.Host);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPASSWORD={0}", ci.Pass);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPORT={0}", ci.Port);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("shp2pgsql -I -s {0} {1} {2} | psql -U {3} -h {4} -p {5} -d {6}", SRID, pathSHP, schemaTable, ci.User, ci.Host, ci.Port, ci.Database);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"SELECT geogis.merge_boundaries_reflect('{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')\"", ci.Database, ci.User, ci.Host, ci.Port,
                schemaTable, "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", false, false, 0, 0, 
                "["+string.Join(",", documents.ToArray()) +"]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        stringBuilder.AppendLine();
            stringBuilder.AppendFormat("pause");
            string filename = "Boundaries_Import_SHP_" + Guid.NewGuid().ToString() + ".bat";

            File.WriteAllText(saveFolder + "\\" + filename, stringBuilder.ToString());
            return filename;
        }
        public static string CreateBatchfileImportContruction(string saveFolder, string sConnection,
string pathSHP,
string schemaTable,
string address,
string chu_dau_tu,
string quy_dau_tu,
string loai_hinh,
string quy_mo,
string trang_thai,
string name,
string avatar,
List<string> images,
string SRID = "4326",
string pgVersion = "12"
)
        {
            ConnectInfo ci = new ConnectInfo(sConnection);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("set root=C:\\Program Files\\PostgreSQL\\{0}\\bin", pgVersion);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("CD /D %root%");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGHOST={0}", ci.Host);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPASSWORD={0}", ci.Pass);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPORT={0}", ci.Port);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("shp2pgsql -I -s {0} {1} {2} | psql -U {3} -h {4} -p {5} -d {6}", SRID, pathSHP, schemaTable, ci.User, ci.Host, ci.Port, ci.Database);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"SELECT geogis.merge_boundaries_contructtion('{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')\"",
                ci.Database, ci.User, ci.Host, ci.Port,
                schemaTable, "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), address, chu_dau_tu, quy_dau_tu, loai_hinh, quy_mo,trang_thai,name,avatar,
                "[" + string.Join(",", images.ToArray()) + "]");
            stringBuilder.AppendLine();
            string filename = "Boundaries_Import_SHP_" + Guid.NewGuid().ToString() + ".bat";

            File.WriteAllText(saveFolder + "\\" + filename, stringBuilder.ToString());
            return filename;
        }
        public static string CreateBatchfileImportSHPAddCols(string saveFolder, string sConnection,
            string pathSHP, 
            string tbname,
            string SRID = "4326",
            string pgVersion = "12")
        {
            ConnectInfo ci = new ConnectInfo(sConnection);
            string s = "";
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("set root=C:\\Program Files\\PostgreSQL\\{0}\\bin", pgVersion);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("CD /D %root%");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGHOST={0}", ci.Host);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPASSWORD={0}", ci.Pass);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPORT={0}", ci.Port);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("shp2pgsql -I -s {0} {1} {2} | psql -U {3} -h {4} -p {5} -d {6}", SRID, pathSHP, tbname, ci.User, ci.Host, ci.Port, ci.Database);
            stringBuilder.AppendLine();
            //stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"alter table {4} add column if not exists ten_loai_dat character varying(254);alter table {4} add column if not exists chu_giai character varying(254)\"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            //stringBuilder.AppendLine();
            //stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"UPDATE {4} A SET ten_loai_dat = B.name FROM cms.land_type_detail B WHERE  A.layer = B.code;UPDATE {4} A SET chu_giai = B.code FROM cms.land_type B INNER JOIN cms.land_type_detail C ON C.land_type_id = B.id WHERE  A.layer = C.code\"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            //stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"alter table {4} add column if not exists style character varying(254) DEFAULT '1' \"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"alter table {4} add column if not exists name character varying(254) DEFAULT '' \"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            stringBuilder.AppendLine();
            string filename = "B1_Import_SHP_" + Guid.NewGuid().ToString() + ".bat";


            File.WriteAllText(saveFolder + "\\" + filename, stringBuilder.ToString());
            return filename;
        }

        public static string RunBatchFile(string folderWorking, string batchfileName)
        {
            try
            {
                folderWorking = folderWorking.Replace("/", "\\").Replace("\\\\", "\\");
                var path = folderWorking + batchfileName;

                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
                processStartInfo.Verb = "runas";
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = true;
                processStartInfo.Arguments = "/c " + path;
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "00|Success";
        }

        public static string RunBatchFile(string batchfileName)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
                processStartInfo.Verb = "runas";
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = true;
                processStartInfo.Arguments = "/c " + batchfileName;
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "00|Success";
        }
        public static string CreateBatchfileAddTablePlace(string saveFolder, string sConnection,
    string tbname,
    string pgVersion = "9.6")
        {
            ConnectInfo ci = new ConnectInfo(sConnection);
            string sql = string.Format("    CREATE SEQUENCE geogis.{0}_id_seq;                                              "
+ "            CREATE TABLE geogis.{0}                                                 "
+ "            (                                                                                           "
+ "                id bigint NOT NULL DEFAULT nextval('geogis.{0}_id_seq'::regclass),  "
+ "                name text COLLATE pg_catalog.\"default\",                                                 "
+ "            lat double precision NOT NULL,                                                              "
+ "            \"long\" double precision NOT NULL,                                                           "
+ "                type text COLLATE pg_catalog.\"default\",                                                 "
+ "                address text COLLATE pg_catalog.\"default\",                                              "
+ "                document_upload_id text COLLATE pg_catalog.\"default\",                                   "
+ "            phone_number text COLLATE pg_catalog.\"default\",                                             "
+ "                email text COLLATE pg_catalog.\"default\",                                                "
+ "                geom geometry,place_id double precision NOT NULL,avatar text,images text,                                                                            "
+ "                CONSTRAINT \"PK_{0}\" PRIMARY KEY(id)                                 "
+ "            )                                                                                           "
+ "WITH(                                                                                                   "
+ "    OIDS = FALSE                                                                                        "
+ ")                                                                                                       "
+ "TABLESPACE pg_default;                                                                                  "
+ "                                                                                                        "
+ "            ALTER TABLE geogis.{0}                                                  "
+ "                OWNER to postgres;                                                                      ", tbname);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("set root=C:\\Program Files\\PostgreSQL\\{0}\\bin", pgVersion);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("CD /D %root%");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGHOST={0}", ci.Host);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPASSWORD={0}", ci.Pass);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("SET PGPORT={0}", ci.Port);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \""+sql+"\"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            stringBuilder.AppendLine();
            //stringBuilder.AppendFormat("psql -d {0} -U {1} -h {2} -p {3} -c \"alter table {4} add column if not exists ten_loai_dat character varying(254)\"", ci.Database, ci.User, ci.Host, ci.Port, tbname);
            //stringBuilder.AppendLine();


            string filename = "B1_Import_SHP_" + Guid.NewGuid().ToString() + ".bat";


            File.WriteAllText(saveFolder + "\\" + filename, stringBuilder.ToString());
            return filename;
        }
    }
}
