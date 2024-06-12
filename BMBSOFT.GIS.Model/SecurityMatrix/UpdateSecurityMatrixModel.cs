using System.Collections.Generic;

namespace BASE.Model.SecurityMatrix
{
    public class UpdateSecurityMatrixModel
    {
        public int Id { set; get; }
        public string RoleId { set; get; }
        public int ScreenId { set; get; }
        public List<int> Actions { set; get; }
    }
}
