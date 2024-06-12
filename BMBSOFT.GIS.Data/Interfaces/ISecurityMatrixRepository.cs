using BASE.Data.Repository;
using BASE.Entity.SecurityMatrix;

namespace BASE.Data.Interfaces
{
    public interface ISecurityMatrixRepository : IBaseRepository<SecurityMatrix>
    {
        bool CheckPermission(string roleName,string actionName,string screenName);
    }
}
