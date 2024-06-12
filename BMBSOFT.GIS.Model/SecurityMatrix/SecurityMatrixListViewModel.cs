using System.Collections.Generic;

namespace BASE.Model.SecurityMatrix
{
    public class SecurityMatrixListViewModel
    {
        public int Id { set; get; }
        public string RoleName { set; get; }
        public string RoleId { set; get; }
        public int ScreenId { set; get; }
        public string ScreenName { set; get; }
        public List<ActionLookupModel> Actions { set; get; }
    }
}
