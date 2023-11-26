using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;
        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _databaseContext.Users
                .Where(x => x.NormalizedEmail == email.ToUpper())
                .Include(x => x.Claims)
                .Include(x => x.Profile)
                    .ThenInclude(x => x.Area)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new EntityNotFoundException(AuthenticationExceptionConstants.UserNotFound, nameof(User), email);
            }

            return user;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _databaseContext.Users
                .Where(x => x.Id == id)
                .Include(x => x.SubscriptionPlan)
                .Include(x => x.Projects)
                .Include(x => x.Company)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                        .ThenInclude(x => x.Claims)
                .Include(x => x.Claims)
                .Include(x => x.Profile)
                    .ThenInclude(x => x.Area)
                .Include(x => x.Profile)
                    .ThenInclude(x => x.ProfileAccessDefinitions)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new EntityNotFoundException(AuthenticationExceptionConstants.UserNotFound, nameof(User), id);
            }

            return user;
        }

        public async Task<bool> IsPhoneNumberExist(string phoneNumber)
        {
            return await _databaseContext.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);            
        }



        public async Task<ListRepositoryModel<User>> GetListAsyc(string? displayName,
                                                                string? email,
                                                                bool? emailConfirmed,
                                                                string? phoneNumber,
                                                                bool? phoneNumberConfirmed,
                                                                bool? lockoutEnabled,
                                                                string? role,
                                                                int pageNumber,
                                                                int pageSize)
        {
            var query = _databaseContext.Users
                .Where(x =>
                    (string.IsNullOrEmpty(displayName) || x.DisplayName.Contains(displayName)) &&
                    (string.IsNullOrEmpty(email) || x.Email.Contains(email)) &&
                    (!emailConfirmed.HasValue || x.EmailConfirmed == emailConfirmed.Value) &&
                    (string.IsNullOrEmpty(phoneNumber) || x.PhoneNumber.Contains(phoneNumber)) &&
                    (!phoneNumberConfirmed.HasValue || x.PhoneNumberConfirmed == phoneNumberConfirmed.Value) &&
                    (!lockoutEnabled.HasValue || x.LockoutEnabled == lockoutEnabled.Value) &&
                    (string.IsNullOrEmpty(role) || x.Roles.Any(y => y.Role.Name.Contains(role)))
                )
                .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .ToListAsync();

            return new ListRepositoryModel<User>(totalRecords, list);
        }

        public async Task<ListRepositoryModel<User>> GetConsultantListAsyc(string? displayName,
                                                                string? email,
                                                                string? phoneNumber,
                                                                int pageNumber,
                                                                int pageSize)
        {

            displayName = !string.IsNullOrEmpty(displayName) ? displayName.ToLower() : null;

            var query = _databaseContext.Users 
                .Where(x =>
                    x.UserType == UserTypes.Consultant &&
                    (string.IsNullOrEmpty(displayName) || x.DisplayName.ToLower().Contains(displayName)) &&
                    (string.IsNullOrEmpty(email) || x.Email.Contains(email)) &&
                    (string.IsNullOrEmpty(phoneNumber) || x.PhoneNumber.Contains(phoneNumber))
                )
                .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .ToListAsync();

            return new ListRepositoryModel<User>(totalRecords, list);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<ListRepositoryModel<User>> GetSubUserListAsyc(int companyId, string? displayName, string? email, string? phoneNumber, UserStatus? status, int pageNumber, int pageSize)
        {
            var query = _databaseContext.Users
               .Where(x => x.CompanyId == companyId && (x.UserType == UserTypes.SubConsultant || x.UserType == UserTypes.SubContractor || x.UserType == UserTypes.SubOwner ) 
               && (string.IsNullOrEmpty(displayName) || x.DisplayName.Contains(displayName)) &&
                   (string.IsNullOrEmpty(email) || x.Email.Contains(email)) && 
                   (string.IsNullOrEmpty(phoneNumber) || x.PhoneNumber.Contains(phoneNumber)) &&
                   (status == null || x.Status == status)
            )
            .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .ToListAsync();

            return new ListRepositoryModel<User>(totalRecords, list);
        }

        public async Task<List<User>> GetCompanyUsersAsync(int companyId)
        {
            return await _databaseContext.Users.Where(x=>x.CompanyId == companyId).ToListAsync();
        }

        public async Task<List<User>> GetByIdAsync(List<string> idList)
        {
            return await _databaseContext.Users.Where(x => idList.Contains(x.Id)).ToListAsync();
        }

        public async Task<ListRepositoryModel<User>> GetContractorListAsyc(string? displayName, int pageNumber, int pageSize)
        {
            displayName = !string.IsNullOrEmpty(displayName) ? displayName.ToLower() : null;

            var query = _databaseContext.Users
                .Where(x =>
                    x.UserType == UserTypes.Contractor &&
                    (string.IsNullOrEmpty(displayName) || x.DisplayName.ToLower().Contains(displayName)) 
                )
                .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize) 
                .ToListAsync();

            return new ListRepositoryModel<User>(totalRecords, list);
        }
    }
}
