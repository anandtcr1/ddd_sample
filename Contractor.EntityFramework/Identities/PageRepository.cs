using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class PageRepository : IPageRepository
    {
        private readonly DatabaseContext _databaseContext;
        public PageRepository (DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<Page>> GetByName(List<string> names)
        {
            return await _databaseContext.Pages
                .Where(x => !x.ParentId.HasValue && names.Contains(x.Name))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Page> GetAsync(int id)
        {
            var page = await _databaseContext.Pages
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

            if(page == null)
        {
                throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Page), id);
            }

            return page;
        }

        public async Task<List<Page>> GetListAsync(List<int> ids)
        {
            var pages = await _databaseContext.Pages
                .Where(x => ids.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync();

            if (ids.Count != pages.Count)
            {
                var idsString = string.Join(", ", ids.Select(e => e.ToString()));
                throw new EntityNotFoundException(nameof(Page), idsString);
            }

            return pages;
        }

        public async Task<ListRepositoryModel<Page>> GetListAsync(string? name, int pageNumber, int pageSize)
        {
            name = !string.IsNullOrEmpty(name) ? name.ToLower() : null;
            var query = _databaseContext.Pages
                        .Where(x => 
                                (string.IsNullOrEmpty(name) || x.Name.Contains(name))
                              );

            var totalRecords = await query.CountAsync();
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Children)
                .ToListAsync();

            return new ListRepositoryModel<Page>(totalRecords, list);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
