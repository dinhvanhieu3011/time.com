namespace BASE.CORE.Helper
{
    public class RoleHelper
    {
        public const string Admin = "ADMIN";
        public class Action
        {
            public const string FullPermission = "FULL_PERMISSION";
            public const string View = "VIEW";
            public const string Create = "CREATE";
            public const string Update = "UPDATE";
            public const string Delete = "DELETE";
            public const string Download = "DOWNLOAD";
        }
        public class Function
        {
            public const string ListLicenseFile = "LIST_LICENSE_FILE";
            public const string MapLicenseFile = "MAP_LICENSE_FILE";
            public const string AddLicenseFile = "ADD_LICENSE_FILE";
            public const string ListProject = "LIST_PROJECT";
            public const string MapProject = "MAP_PROJECT";
        }
        public class Screen
        {
            public const string RoleManagement = "ROLE_MANAGEMENT";
            public const string EmailTemplate = "EMAIL_TEMPLATE";
            public const string UserManagement = "USER_MANAGENENT";
            public const string SecurityMatrix = "SECURITY_MATRIX";
            //public const string Commune = "COMMUNE";
            //public const string District = "DISTRICT";
            //public const string Province = "PROVINCE";
            //public const string Contact = "CONTACT";
            public const string Planning = "PLANNING";
            //public const string ConsultantCommunity = "CONSULTANT_COMUNITY";
            //public const string Statement = "STATEMENT";
            public const string StatementAdmin = "STATEMENT_ADMIN";
            public const string ConsultantCommunityAdmin = "CONSULTANT_ADMIN";
            public const string DocumentManagement = "DOCUMENT_MANAGEMENT";
            public const string RelatedPlanning = "RELATED_PLANNING";

        }
    }
}
