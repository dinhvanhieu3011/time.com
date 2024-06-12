using System;
using System.ComponentModel.DataAnnotations;

namespace BASE.Model.User
{
    public class CreateUserModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        public long DocumentUploadId {get;set;}
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public long? UnitId { get; set; }
    }
}
