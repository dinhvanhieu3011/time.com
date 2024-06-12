using System.Collections.Generic;

namespace BASE.Model.User
{
    public class CurrentUserModel
    {
        public string Id { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }
        public long? UnitId { set; get; }
        public List<RoleModel> Roles { set; get; }
        public List<long> ListChildUnit { set; get; }
    }

    public class RoleModel
    {
        public string Code { set; get; }
        public string Id { set; get; }
    }
}
