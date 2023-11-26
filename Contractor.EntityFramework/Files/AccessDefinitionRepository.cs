using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Files
{
    public class AccessDefinitionRepository : IAccessDefinitionRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AccessDefinitionRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public async Task<AccessDefinition> CreateAsync(AccessDefinition file)
        {
            await _databaseContext.AccessDefinitions.AddAsync(file);

            return file;
        }

        public async Task<List<AccessDefinition>> CreateAsync(List<AccessDefinition> accessDefinitionList)
        {
            await _databaseContext.AccessDefinitions.AddRangeAsync(accessDefinitionList);

            return accessDefinitionList;
        }

        public async Task DeleteAsync(int id)
        {
            var accessDefinition = await _databaseContext.AccessDefinitions.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(AccessDefinition), id);

            _databaseContext.AccessDefinitions.Remove(accessDefinition);
        }

        public async Task<AccessDefinition> GetAsync(int id)
        {
            return await _databaseContext.AccessDefinitions
                .Where(x => x.Id == id)
                .Include(x => x.Children)
                .Include(x => x.Copies)
                .Include(x => x.File)
                .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(AccessDefinition), id);
        }

        public async Task<List<AccessDefinition>> GetAsync(List<int> ids)
        {
            return await _databaseContext.AccessDefinitions
                .Where(x => ids.Contains(x.Id))
                .Include(x => x.File)
                .ToListAsync();
        }

        public async Task<List<AccessDefinition>> GetRouteAsync(string userId)
        {
            return await _databaseContext.AccessDefinitions
                .Where(x => x.UserId == userId && x.ParentId == null)
                .ToListAsync();
        }

        public async Task<List<AccessDefinition>> GetByParentIdAsync(int parentId)
        {
            return await _databaseContext.AccessDefinitions
                .Where(x => x.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<AccessDefinition>> GetByPathAsync(string userId, string path)
        {
            return await _databaseContext.AccessDefinitions
                .Where(x => x.UserId == userId && x.Path.StartsWith(path) && x.ParentId == null)
                .ToListAsync();
        }

        public async Task<AccessDefinition> GetByPathAsync(string path)
        {
            return await _databaseContext.AccessDefinitions
                .Where(x => x.Path == path && x.IsOriginal)
                .Include(x => x.Children)
                .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(AccessDefinition), path);
        }
    }
}
