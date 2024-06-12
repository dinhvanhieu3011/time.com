using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using BASE.CORE.Extensions;
using BASE.Data.Interfaces;
using BASE.Entity.IdentityAccess;
using BASE.Infrastructure.Implements;
using BASE.Infrastructure.Interface;
using BASE.Model.User;
using BASE.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BASE.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment _env;

        public UserService(IUserRepository userRepository, IHostingEnvironment env)
        {
            _userRepository = userRepository;
            _env = env;
        }

        public User GetByUserid(string id)
        {
            return _userRepository
                .Query(x => x.Id == id).FirstOrDefault();
        }

        public string GetRoleByUserId(string id)
        {
            return _userRepository
                .Query(x => x.Id == id)
                .Include(x => x.UserRoles)
                .ThenInclude(e => e.Role).Select(x => x.UserRoles.Select(e => e.Role.Code).FirstOrDefault())
                .FirstOrDefault();
        }

        public IPagedList<UserListViewModel> List(int pageIndex, int pageSize, string sortExpression, string email)
        {
            try
            {
                var filterParams = BuildParams(email);
                var data = _userRepository.Query(x=>x.IsDelete == false).Include(x => x.UserRoles).ThenInclude(e => e.Role).ToList();

                return data.Select(e => new UserListViewModel
                    {
                        Id = e.Id,
                        Email = e.Email,
                        CreatedDate = e.CreatedDate,
                        Description = e.Description,
                        CreatedBy = e.CreatedBy,
                        ModifiedBy = e.ModifiedBy,
                        Status = e.Status,
                        Address = e.Address,
                        DateOfBirth = e.DateOfBirth,
                        FullName = e.FullName,
                        ModifiedDate = e.ModifiedDate,
                        Sex = e.Sex,
                        RoleName = e.UserRoles.Select(r => r.Role.Name).FirstOrDefault(),
                        PhoneNumber = e.PhoneNumber
                    }).AsEnumerable().FilteredData(filterParams).AsQueryable()
                    .Sort(sortExpression ?? "CreatedDate desc")
                    .ToPagedList(pageIndex, pageSize);
                //return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<UserLookupModel> GetLookupUser(long? unitId)
        {
            var data = _userRepository.GetAll()
                .Where(x => x.Status).ToList().Select(x=> new UserLookupModel
            {
                Id =x.Id,
                UserName = x.UserName,
                Email = x.Email,
            }).ToList();
            return data;
        }
        public DetailUserModel GetUserDetail(string id)
        {
            var data = _userRepository
                .Query(x => x.Id == id).Include(x => x.UserRoles).ThenInclude(e => e.Role);
            var result = data.ToList().Select(x =>
                    new DetailUserModel
                    {
                        Id = x.Id,
                        RoleId = x.UserRoles.Select(e => e.Role.Id).FirstOrDefault(),
                        Email = x.Email,
                        ModifiedBy = x.ModifiedBy,
                        Description = x.Description,
                        CreatedDate = x.CreatedDate,
                        CreatedBy = x.CreatedBy,
                        FullName = x.FullName,
                        Status = x.Status,
                        DateOfBirth = x.DateOfBirth,
                        Sex = x.Sex,
                        Address = x.Address,
                        ModifiedDate = x.ModifiedDate,
                        PhoneNumber = x.PhoneNumber,
                    }).FirstOrDefault();
            return result;
        }

        #region private

        private static List<FilterExtensions.FilterParams> BuildParams(string email)
        {
            var filterParams = new List<FilterExtensions.FilterParams>();
            if (!string.IsNullOrEmpty(email))
                filterParams.Add(new FilterExtensions.FilterParams
                {
                    ColumnName = "Email",
                    FilterValue = email,
                    FilterOption = FilterExtensions.FilterOptions.Contains
                });
            return filterParams;
        }

        #endregion
    }
}