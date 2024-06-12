using System;
using BASE.Entity.LogHistory;
using BASE.Infrastructure.Interface;
using BASE.Model.LogHistory;
using BASE.Model.User;

namespace BASE.Services.Interfaces
{
    public interface ILogHistoryService
    {
        bool Create(LogHistoryModel logHistoryModel, CurrentUserModel currentUserModel);
        IPagedList<LogHistoryModelView> GetAllLogHistory(int pageIndex, int pageSize, string sortExpression, int action, string userName, string description, DateTime? createDate);
        bool UpdateLogHistory(LogHistoryModel logHistoryModel, CurrentUserModel currentUserModel);
        bool DeleteLogHistoryById(int idLogHistory);
        LogHistoryEntity GetLogHistoryById(int id);
    }
}
