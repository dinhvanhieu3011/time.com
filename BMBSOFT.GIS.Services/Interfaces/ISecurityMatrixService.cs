using System.Collections.Generic;
using BASE.Infrastructure.Interface;
using BASE.Model.SecurityMatrix;

namespace BASE.Services.Interfaces
{
    public interface ISecurityMatrixService 
    {
        List<ActionLookupModel> GetActionLookup();
        List<ScreenLookupModel> GetScreenLookup();
        IPagedList<SecurityMatrixListViewModel> GetListSecurityMatrix(int pageIndex,int pageSize,string sortExpression,string roleName,string screenName);
        List<ScreenViewModel> GetDetailSecurityMatrix(string Id);
        bool CreateSecurityMatrix(CreateSecurityMatrixModel model);
        bool UpdateSecurityMatrix(CreateSecurityMatrixModel model);
        bool CheckPermission(string roleId, string actionName, string screenName);
    }
}
