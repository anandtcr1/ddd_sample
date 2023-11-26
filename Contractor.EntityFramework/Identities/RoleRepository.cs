using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Contractor.Identities
{
    public class RoleRepository : IRoleRepository
    {
        DatabaseContext _databaseContext;
        public RoleRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="name"></param>
        /// <param name="normalizedName"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public async Task<ListRepositoryModel<Role>> GetListAsync(string? name,
            bool? isActive,
            int pageNumber, int pageSize)
        {
            var query = _databaseContext.Roles
                        .Where(x =>
                            (string.IsNullOrEmpty(name) || x.Name.Contains(name)) &&
                            (!isActive.HasValue || x.IsActive == isActive)
                        )
                        .AsNoTracking();

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ListRepositoryModel<Role>(totalRecords, list);
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public async Task<List<Role>> GetAsync(List<string> names)
        {
            return await _databaseContext.Roles
                .Where(x=>  names.Contains(x.Name))
                .Include(x=>x.Claims)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _databaseContext.Roles
                 .Where(x => x.NormalizedName == name.ToUpper())
                 .Include(x => x.Claims)
                 .FirstOrDefaultAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(string id)
        {
            var role = await _databaseContext.Roles
                 .Where(x => x.Id == id)
                 .Include(x => x.Claims)
                 .FirstOrDefaultAsync();

            return role == null ? throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Role), id) : role;
        }

        public async Task<List<Role>> GetRoleByIdAsync(List<string> ids)
        {
            return await _databaseContext.Roles
                 .Where(x => ids.Contains(x.Id))
                 .Include(x => x.Claims)
                 .ToListAsync();            
        }

        public async Task<List<Role>> GetAll(string search)
        {
            return await _databaseContext.Roles
                .Where(x => (string.IsNullOrEmpty(search) || x.Name.Contains(search)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _databaseContext.SaveChangesAsync();
        }
    }
}
