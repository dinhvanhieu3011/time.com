using System;

namespace BASE.Model.User
{
    public class UpdateUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        //public string Avatar { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public long DocumentUploadId {get;set;}
        public long? UnitId { get;set;}
    }    
}
