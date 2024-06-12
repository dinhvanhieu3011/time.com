using System;

namespace BASE.Model.User
{
    public class UserListViewModel
    {
        public string Id { set; get; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Sex { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string RoleName { set; get; }
        public string PhoneNumber { set; get; }
    }
}
