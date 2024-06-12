using BASE.Entity.IdentityAccess;
using BASE.Infrastructure.Interface;
using BASE.Model.User;
using System.Collections.Generic;

namespace BASE.Services.Interfaces
{
    public interface IUserService
    {
        User GetByUserid(string id);
        string GetRoleByUserId(string id);
        IPagedList<UserListViewModel> List(int pageIndex, int pageSize, string sortExpression, string email);
        DetailUserModel GetUserDetail(string id);
        List<UserLookupModel> GetLookupUser(long? unitId);
    }
}
