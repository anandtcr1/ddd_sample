using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Correspondences
{
    public class CorrespondenceRepository : ICorrespondenceRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CorrespondenceRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Correspondence> CreateAsync(Correspondence correspondence)
        {
            await _databaseContext.Correspondences.AddAsync(correspondence);

            return correspondence;
        }

        public async Task<CorrespondenceThread> CreateThreadAsync(CorrespondenceThread correspondenceThread)
        {
            await _databaseContext.CorrespondenceThreads.AddAsync(correspondenceThread);

            return correspondenceThread;
        }

        public async Task<Correspondence> GetAsync(int id)
        {
            return await _databaseContext.Correspondences
                .Where(x => x.Id == id)
                .Include(x => x.CorrespondenceAccessDefinitions)
                .Include(x => x.CorrespondenceRecipients)
                .FirstOrDefaultAsync() ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(Correspondence), id);
        }

        public async Task<List<Correspondence>> GetByThreadIdAsync(int threadId)
        {
            return await _databaseContext.Correspondences
                .Where(x => x.ThreadId == threadId)
                .Include(x => x.CorrespondenceAccessDefinitions)
                .Include(x => x.CorrespondenceRecipients)
                    .ThenInclude(x => x.Recipient)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ListRepositoryModel<Correspondence>> GetIncomingAsync(string userId, string? search, int pageNumber, int pageSize)
        {
            search = !string.IsNullOrEmpty(search) ? search.ToLower() : null;

            var query = _databaseContext.Correspondences
                .Where(x =>
                    (x.CorrespondenceRecipients.Any(y => y.RecipientId == userId && y.RecipientType == CorrespondenceRecipientType.Sender)) &&
                    (string.IsNullOrEmpty(search) || x.Content.Contains(search)) &&
                    (string.IsNullOrEmpty(search) || x.Subject.Contains(search))
                  ).AsNoTracking();

            var totalRecords = await query.CountAsync();

            var list = await query
                .OrderByDescending(x => x.IssueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.CorrespondenceRecipients)
                    .ThenInclude(x => x.Recipient)
                .ToListAsync();

            return new ListRepositoryModel<Correspondence>(totalRecords, list);
        }

        public async Task<int> GetNextIdAsync()
        {
            return await _databaseContext.Correspondences.AnyAsync() ? await _databaseContext.Correspondences.MaxAsync(x => x.Id) + 1 : 0;
        }

        public async Task<ListRepositoryModel<Correspondence>> GetOutgoingAsync(string userId, string? search, int pageNumber, int pageSize)
        {
            search = !string.IsNullOrEmpty(search) ? search.ToLower() : null;

            var query = _databaseContext.Correspondences
                .Where(x =>
                    (x.CorrespondenceRecipients.Any(y => y.RecipientId == userId && (y.RecipientType == CorrespondenceRecipientType.CCReceiver || y.RecipientType == CorrespondenceRecipientType.ToReceiver))) &&
                    (string.IsNullOrEmpty(search) || x.Content.Contains(search)) &&
                    (string.IsNullOrEmpty(search) || x.Subject.Contains(search))
                  ).AsNoTracking();

            var totalRecords = await query.CountAsync();

            var list = await query
                .OrderByDescending(x => x.IssueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.CorrespondenceRecipients)
                    .ThenInclude(x => x.Recipient)
                .ToListAsync();

            return new ListRepositoryModel<Correspondence>(totalRecords, list);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
