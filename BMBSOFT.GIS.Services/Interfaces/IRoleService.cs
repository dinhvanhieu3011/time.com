using System.Collections.Generic;
using BASE.Entity.IdentityAccess;
using BASE.Infrastructure.Interface;
using BASE.Model.Role;

namespace BASE.Services.Interfaces
{
    public interface IRoleService 
    {
        IPagedList<RoleViewModel> List(int pageIndex, int pageSize, string sortExpression, string code, string name);
        RoleViewModel GetDetail(string id);
        IList<RoleLookupModel> GetRoleLookup();
        Role GetById(string id);
        bool GetByCode(string code);
        bool CreateRole(Role entity);
        bool UpdateRole(Role entity);
        bool RemoveRole(string id);
        string GetRoleNameById(string id);
        bool CheckRoleById(string id, string roleCode);
    }
}
