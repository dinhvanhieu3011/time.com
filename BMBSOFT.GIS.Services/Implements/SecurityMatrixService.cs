using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using BASE.CORE.Extensions;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Infrastructure.Implements;
using BASE.Entity.SecurityMatrix;
using BASE.Infrastructure.Interface;
using BASE.Model.SecurityMatrix;
using BASE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BASE.Services.Implements
{
    public class SecurityMatrixService : ISecurityMatrixService
    {
        private readonly ISecurityMatrixRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SecurityMatrixService> _logger;
        private readonly IActionRepository _actionRepository;
        private readonly IScreenRepository _screenRepository;

        public SecurityMatrixService(ILogger<SecurityMatrixService> logger, IUnitOfWork unitOfWork, ISecurityMatrixRepository repository, IActionRepository actionRepository, IScreenRepository screenRepository)
        {
            _repository = repository;
            _actionRepository = actionRepository;
            _screenRepository = screenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public List<ActionLookupModel> GetActionLookup()
        {
            return _actionRepository.Query(x => x.Id > 0).Select(x => new ActionLookupModel
            {
                Name = x.Name,
                Id = x.Id
            }).ToList();
        }

        public List<ScreenLookupModel> GetScreenLookup()
        {
            return _screenRepository.Query(x => x.Id > 0).Select(x => new ScreenLookupModel
            {
                Name = x.Name,
                Id = x.Id
            }).ToList();
        }

        public IPagedList<SecurityMatrixListViewModel> GetListSecurityMatrix(int pageIndex, int pageSize, string sortExpression, string roleName,
            string screenName)
        {
            var data = _repository.GetAll()
                .Include(e => e.Screen)
                .Include(e => e.Action)
                .Include(e => e.Role).GroupBy(x => new
                {
                    x.Id,
                    x.RoleId,
                    RoleName = x.Role.Name,
                    ScreenName = x.Screen.Name,
                    x.ScreenId
                });
            var filterParams = MapParams(roleName, screenName);

            var rs = data.Select(x => new SecurityMatrixListViewModel
            {
                Id = x.Key.Id,
                ScreenId = x.Key.ScreenId,
                RoleId = x.Key.RoleId,
                RoleName = x.Key.RoleName,
                ScreenName = x.Key.ScreenName,
                Actions = _repository.GetAll().Include(securityMatrix => securityMatrix.Action).Where(e => e.Id == x.Key.Id).Select(e => new ActionLookupModel
                {
                    Id = e.Action.Id,
                    Name = e.Action.Name
                }).ToList()
            }).ToList();
            return rs.FilteredData(filterParams).AsQueryable().Sort(sortExpression ?? "RoleName asc").ToPagedList(pageIndex, pageSize);
        }

        public List<ScreenViewModel> GetDetailSecurityMatrix(string RoleId)
        {
            var listScreen = _repository.Query(x => x.RoleId == RoleId).Include(x=>x.Screen).Select(x=> new ScreenViewModel
            {
                ScreenId = x.ScreenId,
                ScreenName = x.Screen.Name
            });
            var screen = listScreen.Distinct().ToList();
            foreach (var entry in screen)
            {
                var listAction = _repository.Query(x => x.ScreenId == entry.ScreenId && x.RoleId == RoleId).Select(x => new ActionViewModel
                {
                    ActionId = x.ActionId,
                    ActionName = x.Action.Name
                }).ToList();
                entry.Actions = listAction;
            }
            return screen;
        }

        public bool CreateSecurityMatrix(CreateSecurityMatrixModel model)
        {
            using var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                if (!model.Screens.Any())
                {
                    return false;
                }

                foreach (var entry in from screen in model.Screens
                                      from action in screen.Actions
                                      select new SecurityMatrix
                                      {
                                          RoleId = model.RoleId,
                                          ScreenId = screen.ScreenId,
                                          ActionId = action.ActionId
                                      })
                {
                    _repository.Insert(entry);
                }
                dbTransaction.Commit();
                _unitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        public bool UpdateSecurityMatrix(CreateSecurityMatrixModel model)
        {
            using var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                var deleted = _repository.Query(x => x.RoleId == model.RoleId).ToList();
                if (!model.Screens.Any())
                {
                    _repository.DeleteMulti(deleted);
                    dbTransaction.Commit();
                    _unitOfWork.Complete();
                    return true;
                }

                _repository.DeleteMulti(deleted);
                foreach (var screen in model.Screens)
                {
                    var listActions = screen.Actions.Distinct().ToList();
                    foreach (var action in listActions)
                    {
                        var entry = new SecurityMatrix
                        {
                            RoleId =  model.RoleId,
                            ScreenId = screen.ScreenId,
                            ActionId = action.ActionId
                        };
                        _repository.Insert(entry);
                    }
                }
                dbTransaction.Commit();
                _unitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
        }

        public bool CheckPermission(string roleName, string actionName, string screenName)
        {
            return _repository.CheckPermission(roleName, actionName, screenName);
        }

        #region Private

        private List<FilterExtensions.FilterParams> MapParams(string roleName, string screenName)
        {
            var filterParams = new List<FilterExtensions.FilterParams>();
            if (!string.IsNullOrEmpty(roleName))
                filterParams.Add(new FilterExtensions.FilterParams()
                {
                    ColumnName = "RoleName",
                    FilterValue = roleName
                });
            if (!string.IsNullOrEmpty(screenName))
                filterParams.Add(new FilterExtensions.FilterParams()
                {
                    ColumnName = "ScreenName",
                    FilterValue = screenName
                });
            return filterParams;
        }

        #endregion
    }
}
