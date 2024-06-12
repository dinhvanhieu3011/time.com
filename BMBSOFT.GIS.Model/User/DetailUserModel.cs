using System;

namespace BASE.Model.User
{
    public class DetailUserModel
    {
        public string Id { set; get; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public Files Files { get; set; }
        public long? UnitId { get; set; }
    }

    //public class Files
    //{
    //    public string fileName { get; set; }
    //    public string filePreview { get; set; }
    //    public string fileType { get; set; }
    //    public long fileSize { get; set; }
    //    public long? fileId { get; set; }
    //}
    public class DetailUserCmsModel
    {        
        public string Id {get;set;}
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }        
        public string PhoneNumber { get; set; }
    }
    public class UpdateUserCmsModel{
        public string Id { set; get; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        //public long? DocumentUploadId {get;set;}
    }
}
