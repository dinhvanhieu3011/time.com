using System;

namespace BMBSOFT.GIS.CORE.Helper
{
    public class Constant
    {
        public const string DisplayName = "PQLĐT Vĩnh Long";
        public const int DefaultPageSize = 20;
        public const int DefaultPageIndex = 1;
        public const int FileNameMaxLength = 100;
        public const int FileSizeMax = 5242880; //2MB
        public const string GEMBOX_EXCEL = "FREE-LIMITED-KEY"; 
        public const string GEMBOX_WORD = "FREE-LIMITED-KEY"; 
       public class ResponseCode
        {
            public const string E00 = "Thành công";
            public const string E01 = "Da ton tai PageType";
        }
        public class AnalysisAddressMessage
        {
            public const string CreateBy = "Được tạo từ AnalysisAddress";
            //public const string Url = "https://maps.googleapis.com/maps/api/geocode/json?{0}&key={1}";
            public const string Url = "https://api.map4d.vn/sdk/v2/geocode?{0}&key={1}";
        }
        public class Schema
        {
            public const string CMS = "cms";
            public const string DWH = "dwh";
            public const string SYS = "sys";
            public const string GEOGIS = "geogis";
            public const string AUTHENTICATION = "authentication";
            public const string PAHT = "paht";
            public const string CONSTRUCTION_FLUCTUATION = "construction_fluctuations";
        }
        public static class Module
        {
            public const string EmailTemplate = "Email Template";
        }

        public static class EmailType
        {
            public const int Register = 1;
            public const int ForgotPassWord = 1;
        }
        public class StatusCode
        {
            public const int _200 = 200;
            public const int _400 = 400;
        }

        public class Status
        {
            public const string ADD = "ADD";
            public const string UPDATE = "UPDATE";
            public const string DELETE = "DELETE";
            public const string NONE = "NONE";

        }

        public class MAP
        {
            public const string MAP_NAME = "Bản đồ nền";
            public const string MAP_TYPE = "MAP";
            public const string LAYER_TYPE = "LAYER";
            public const string LAYER_NAME = "LAYER";
        }
        public class EmailTemplate
        {
            public const string ContactToAdmin = "CONTACTADMIN";
            public const string ContactToUser = "CONTACTUSER";
            public const string ForgotPassword = "FORGOTPASSWORD";
            public const string ChangedPassword = "CHANGEDPASSWORD";
            public const string CreateUser = "CREATEUSER";
            public const string ACTIVEUSER = "ACTIVEUSER";
            public const string Reply = "REPLY";
            public const string ReplyContact = "REPLYCONTACT";
        }
        public class PathName
        {
            public const string CgisFolder = @"\Uploads\cgis";
            public const string UserFolder = @"\Uploads\Ảnh người dùng";
            public const string SliderFolder = @"\Uploads\Trang chủ";
            public const string Planning = @"\Uploads\Ảnh dự án quy hoạch";
            public const string Statement = @"\Uploads\Công bố quy hoạch";
            public const string ConsultantCommunity = @"\Uploads\Xin ý kiến cộng đồng";
            public const string Document = @"\Uploads\document-content";
            public const string DocumentPlanning = @"\Uploads\document-content\{0}";
            public const string News = @"Uploads\Ảnh tin tức";
            public const string ImportExcel = @"Uploads\cgis";
            public const string ReflectTheScence = @"Uploads\reflectthescence";

        }
        public class DocumentType
        {
            public const string Folder = "Folder";
            public const string File = "File";
        }

        public class DefaultFields
        {
            public const string CreatedDate = "CreatedDate";
        }

        public class SortType
        {
            public const string desc = "desc";
        }

        //Max length constant
        public class Maxlength
        {
            public const int Title = 150;
            public const int Color = 7;
            public const int Name = 200;
            public const int Description = 550;
        }
        public class ProjectionInfo
        {
            public const string WGS84 = "+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs";
        }
        public class CoordinateRoundingFactor
        {
            public const int N_DEC_WGS84 = 8;
            public const int N_DEC_VN2000 = 3;
        }

        public class StoreProceduce
        {
            public const string CoordinatePlanning = "cms.get_planning_by_coordinate";
            public const string SearchPlanningByCoordinateWGS84 = "cms.search_planning_by_coordinate_wgs84";
        }
        public static class ReflectionStatus
        {
            public const int Pending = 1;
            public const int Processing = 2;
            public const int Resolve = 3;
        }
        public static class ReflectionAction
        {
            public const string Init = "INIT";
            public const string Receive = "RECEIVE";
            public const string Handle = "HANDLE";
        }
        public static class FileExtentions
        {
            public static readonly string[] FileExtention = { "pdf", "xlsx","docx" };
        }
    }
}
