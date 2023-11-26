using Contractor.EntityFrameworkCore;
using Contractor.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Files
{
    public class FileRepository : IFileRepository
    {
        private readonly DatabaseContext _databaseContext;

        public FileRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<File> CreateAsync(File file)
        {
           await _databaseContext.Files.AddAsync(file);

            return file;
        }

        public async Task<File> GetAsync(int id)
        {
            return await _databaseContext.Files.FirstOrDefaultAsync(x=>x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(File), id);
        }

        public async Task DeleteAsync(int id)
        {
            var file = await _databaseContext.Files.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new EntityNotFoundException(EntityNotFoundExceptionConstants.EntityNotFound, nameof(File), id);

            _databaseContext.Files.Remove(file);
        }

        public async Task DeleteAsync(List<int> ids)
        {
            var files = await _databaseContext.Files
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            _databaseContext.Files.RemoveRange(files);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
