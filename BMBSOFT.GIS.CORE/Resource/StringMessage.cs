namespace BASE.CORE.Resource
{
    public class StringMessage
    {
        public static class ErrorMessages
        {
            public const string EmailNotEmpty = "Email không được để trống!";
            public const string EmailNotValid = "Email không đúng định dạng!";
            public const string TitleNotEmpty = "Title không được để trống!";
            public const string RecordNotFound = "Không có bản ghi nào được tìm thấy.";
            public const string ItemNotFound = "Không có bản ghi nào được tìm thấy {0}.";
            public const string DataNotValid = "Dữ liệu không hợp lệ.";
            public const string ErrorProcess = "Lỗi trong quá trình xử lý dữ liệu.Vui lòng liên hệ với admin.";

            public const string UserAlreadyExits = "{0} đã tồn tại.";
            public const string EmailNotRegister = "Email {0} chưa được đăng kí.";
            public const string PasswordNotValid = "Mật khẩu không đúng.";
            public const string NewPasswordNotValid = "Mật khẩu mới không đúng định dạng.";
            public const string AccountNotValid = "Tài khoản hoặc  mật khẩu không đúng.";
            public const string AccountNotActive = "Tài khoản chưa được phê duyệt.";
            public const string FieldNotExitsInModel = "Trường {0} không hợp lệ.";
            public const string AlreadyExistedRecordMasterCode = "Đã tồn tại page với master code";
            public const string SendMailError = "Lỗi trong quá trình gửi mail";
            public const string IsExistedConsultantCommunity = "Tồn tại xin ý kiến cộng đồng với dự án này.";
            public const string IsExistedStatement = "Tồn tại công bố quy hoạch với dự án này.";
            public const string ExistsEmailTemplateCode = "Đã có email tồn tại với mã code này.";
            public const string ExistsRoleCode = "Đã có Role tồn tại với mã code này.";
            public const string ExistsPlanningRelationShipCode = "Đã có Planning relation ship tồn tại với mã code này.";
            public const string EmailTemplateNotFound = "Email template không được tìm thấy.";
            public const string AccessDenied = "Bạn không có quyền truy cập trang này";
            public const string IsExistedOrder = "Số thứ tự này đã tồn tại";
            public const string PlanningIsUnApproved = "Dự án này chưa được phê duyệt";
            public const string PlanningOnlyConsultantCommunity = "Dự án này đã có bài viết xin ý kiến cộng đồng";
            public const string PlanningOnlyStatement = "Dự án này đã có bài viết công bố";
            public const string FileEmpty = "Không có file nào được tải lên.";
            public const string FileExisted = "File {0}  đã tồn tại.";
            public const string FolderExisted = "Folder {0}  đã tồn tại.";
            public const string NameIsEmpty = "Tên {0} không được bỏ trống.";
            public const string TableExisted = "Bảng {0} đã tồn tại.";
            public const string ContentType = "Content-Type không đúng: tif";
            public const string FileIsNotExist = "File không tồn tại.";
            public const string PlanningExisted = "Hồ sơ đã tồn tại/ hồ sơ đã có trong kho";
            public const string IsExistedMap = "Tồn tại bản đồ đi kèm dự án.";
            public const string PlanningHasDocument = "Dự án có hồ sơ đi kèm";
            public const string DistrictIsUsed = "Quận/Huyện đang được quy hoạch.";
            public const string NoFieldRequire = "Chưa đính kèm ảnh.";
            public const string MapIsDuplicated = "Bản đồ đã tồn tại.";
            public const string FileOrFolderDuplicate = "Tồn tại file/folder trong thư mục đích trùng tên.";
            public const string MailIsNeeded = "Người đóng góp ý kiến không để lại email phản hồi.";
            public const string FileNameTooLong = "Vui lòng đặt tên file tải lên dưới 100 ký tự";
            public const string DocumentIsNotExist = "Hồ sơ dự án không tồn tại.";
            public const string HexColorNotValid = "Mã màu hex không đúng định dạng.";
            public const string CadColorNotValid = "Mã màu CAD không đúng định dạng.";
            public const string FileTooLarge = "Vui lòng tải lên file kích thước không quá 5MB";
            public const string FileCannotDisable = "File/folder nằm trong folder cha bị ẩn.";
            public const string TimeNotValid = "Thời gian bắt đầu cần lớn hơn thời gian kết thúc";
            public const string FileNameDuplicate = "Tên tiff đã tồn tại.";
            public const string NonTiffFile = "Không có file tif nào được tải lên.";
            public const string NonShpFile = "Không có shape file nào được tải lên.";
            public const string FileNotValid = "File tải lên không đúng định dạng {0}";
            public const string DownloadCmsDenied = "Bạn không có quyền tải xuống tài nguyên này.";
            public const string DownloadDenied = "Bạn không có quyền tải xuống tài nguyên {0}.";
            public const string UploadDenied = "Bạn chưa được cấp quyền tải lên";
            public const string ViewDenied = "Bạn chưa được cấp quyền xem tài nguyên này.";
            public const string UpdateDenied = "Bạn chưa được cấp quyền chỉnh sửa.";
            public const string DeleteDenied = "Bạn chưa được cấp quyền xóa tài nguyên này.";
            public const string CreateDenied = "Bạn chưa được cấp quyền tạo mới tài nguyên.";
            public const string NoPermission = "Để truy cập nội dung này, bạn vui lòng liên hệ với cơ quan chủ quan theo như thông tin bên dưới.";
            public const string NameIsNotValid = "Tên file/folder chỉ được chứa chữ, số, ký tự gạch ngang, gạch dưới và khoảng trắng";
            public const string DuplicateScreens = "Màn hình phân quyền bị trùng lặp.";
        }
        public static class SuccessMessage
        {
            public const string CreatedSuccessFully = "{0} đã được tạo thành công !";
            public const string UpdatedSuccessFully = "{0} đã được cập nhập thành công !";
            public const string DeletedSuccessFully = "{0} đã xóa thành công !";
            public const string RegisteredAccountSuccessFully = "Tài khoản của bạn đã được tạo thành công.Và đang chờ phê duyệt.";
            public const string ForgotPassword = "Liên kết thay đổi mật khẩu đã được gửi tới {0}.vui lòng kiểm tra hòm thư của bạn";
            public const string RegisteredFirebaseTokenSuccessFully = "Token đã được lưu thành công";

        }
    }
}
