using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BASE.CORE.Extensions;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.IdentityAccess;
using BASE.Infrastructure.Interface;
using BASE.Infrastructure.Implements;
using BASE.Model.Role;
using BASE.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BASE.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleService> _logger;
        public RoleService(ILogger<RoleService> logger, IRoleRepository roleRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IPagedList<RoleViewModel> List(int pageIndex, int pageSize, string sortExpression, string code, string name)
        {
            var roles = _roleRepository.GetAll();
            var filterParams = BuildParams(code, name);

            return roles?.AsEnumerable().Select(x => _mapper.Map<RoleViewModel>(x))
                .FilteredData(filterParams)
                .AsQueryable()
                .Sort(sortExpression ?? "Code asc")
                .ToPagedList(pageIndex, pageSize);
        }

        public RoleViewModel GetDetail(string id)
        {
            return _roleRepository.Query(x => x.Id == id).Select(x => _mapper.Map<RoleViewModel>(x)).FirstOrDefault();
        }

        public string GetRoleNameById(string id){
            var data = _roleRepository.Find(x => x.Id == id);
            return data != null ?  data.Name : null;               
        }

        public IList<RoleLookupModel> GetRoleLookup()
        {
            return _roleRepository.GetAll().Select(x => new RoleLookupModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public Role GetById(string id)
        {
            return _roleRepository.Query(x => x.Id == id)
                .FirstOrDefault();
        }

        public bool GetByCode(string code)
        {
            return _roleRepository.Query(x => x.Code == code)
                       .FirstOrDefault() != null;
        }

        public bool CreateRole(Role entity)
        {
            var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                //var roleCreate = _roleRepository.GetById(entity.Id);
                //roleCreate.Code = entity.Code;
                //roleCreate.Name = entity.Name;
                //roleCreate.CreatedBy = "admin";
                //roleCreate.CreatedDate = DateTime.Now;
                _roleRepository.Insert(entity);
                _unitOfWork.Complete();
                dbTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                dbTransaction.Rollback();
                return false;
            }
        }

        public bool UpdateRole(Role entity)
        {
            var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                //var roleUpdate = _roleRepository.GetById(entity.Id);
                //roleUpdate.Code = entity.Code;
                //roleUpdate.Name = entity.Name;
                //roleUpdate.ModifiedBy = "admin";
                //roleUpdate.ModifiedDate = DateTime.Now;
                _roleRepository.Update(entity);
                _unitOfWork.Complete();
                dbTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                dbTransaction.Rollback();
                return false;
            }
        }

        public bool RemoveRole(string id)
        {
            var dbTransaction = _unitOfWork.BeginTransaction();
            try
            {
                _roleRepository.Delete(id);
                _unitOfWork.Complete();
                dbTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                dbTransaction.Rollback();
                return false;
            }
        }

        public bool CheckRoleById(string id, string roleCode)
        {
            return _roleRepository.Find(x => x.Id == id)?.Code == roleCode;
        }
        #region private

        private static List<FilterExtensions.FilterParams> BuildParams(string code, string name)
        {
            var filterParams = new List<FilterExtensions.FilterParams>();
            if (!string.IsNullOrEmpty(code))
                filterParams.Add(new FilterExtensions.FilterParams
                {
                    ColumnName = "Code",
                    FilterValue = code,
                    FilterOption = FilterExtensions.FilterOptions.Contains
                });
            if (!string.IsNullOrEmpty(name))
                filterParams.Add(new FilterExtensions.FilterParams()
                {
                    ColumnName = "Name",
                    FilterValue = name,
                    FilterOption = FilterExtensions.FilterOptions.Contains
                });
            return filterParams;
        }

        #endregion
    }
}
