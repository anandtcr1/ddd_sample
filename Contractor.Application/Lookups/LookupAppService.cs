using Contractor.Contracts;
using Contractor.Correspondences;
using Contractor.Identities;
using Contractor.Projects;

namespace Contractor.Lookups
{
    public class LookupAppService: ILookupAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly INationalityRepository _nationalityRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly IOutGoingTypeRepository _outGoingTypeRepository;
        private readonly IIncomeTypeRepository _incomeTypeRepository;
        private readonly IProjectTypeRepository _projectTypeRepository;

        public LookupAppService(IRoleRepository roleRepository,
            INationalityRepository ationalityRepository,
            IContractTypeRepository contractTypeRepository,
            IAddressRepository addressRepository,
            IOutGoingTypeRepository outGoingTypeRepository,
            IIncomeTypeRepository incomeTypeRepository,
            IProjectTypeRepository projectTypeRepository,
            IProjectRepository projectRepository)
        {
            _roleRepository = roleRepository;
            _nationalityRepository = ationalityRepository;
            _addressRepository = addressRepository;
            _contractTypeRepository = contractTypeRepository;
            _outGoingTypeRepository = outGoingTypeRepository;
            _incomeTypeRepository = incomeTypeRepository;
            _projectTypeRepository = projectTypeRepository;
            _projectRepository = projectRepository;
        }

        public async Task<List<Lookup<string>>> GetRoles(string search)
        {
            return (await _roleRepository.GetAll(search))
                .Select(x => new Lookup<string>
                {
                    EnglishDisplay = x.Name,
                    ArabicDisplay = x.Name,
                    Value = x.Id
                }).ToList();
        }

        public async Task<List<Lookup<int>>> GetCities(string? search)
        {
            return (await _addressRepository.GetAllCityAsync(search))
                .Select(x => new Lookup<int>
                {
                    EnglishDisplay = x.EnglishName,
                    ArabicDisplay = x.ArabicName,
                    Value = x.Id
                }).ToList();
        }

        public async Task<List<Lookup<int>>> GetProjects(string userId, string? search)
        {
            return (await _projectRepository.GetAllLiteAsync(userId, search))
                .Select(x => new Lookup<int>
                {
                    EnglishDisplay = x.ProjectNumber,
                    ArabicDisplay = x.ProjectNumber,
                    Value = x.Id
                }).ToList();
        }

        public async Task<List<Lookup<int>>> GetAreas(int cityId, string? search)
        {
            return (await _addressRepository.GetAllAreaAsync(cityId, search))
                .Select(x => new Lookup<int>
                {
                    EnglishDisplay = x.EnglishName,
                    ArabicDisplay = x.ArabicName,
                    Value = x.Id
                }).ToList();
        }

        public async Task<List<Lookup<int>>> GetNationalities(string? search)
        {
            return (await _nationalityRepository.GetAllAsync(search))
                .Select(x => new Lookup<int>
                {
                    EnglishDisplay = x.EnglishName,
                    ArabicDisplay = x.ArabicName,
                    Value = x.Id
                }).ToList();
        }

        public async Task<List<Lookup<int>>> GetContractTypes(string? search)
        {
            // TODO 
            // Return ArabicDescription Also
            return (await _contractTypeRepository.GetAllAsync(search))
                .Select(x => new Lookup<int>
                {
                    Value = x.Id,
                    EnglishDisplay = x.EnglishDescription,
                    ArabicDisplay = x.ArabicDescription,
                }).ToList();
                
        }

        public async Task<List<Lookup<int>>> GetOutGoingTypes(string? search)
        {
            // OutGoingTypes
            return (await _outGoingTypeRepository.GetAllAsync(search))
                .Select(x => new Lookup<int>
                {
                    Value = x.Id,
                    EnglishDisplay = x.EnglishDescription,
                    ArabicDisplay = x.ArabicDescription,
                }).ToList();

        }

        public async Task<List<Lookup<int>>> GetIncomeTypes(string? search)
        {
            // IncomeTypes
            return (await _incomeTypeRepository.GetAllAsync(search))
                .Select(x => new Lookup<int>
                {
                    Value = x.Id,
                    EnglishDisplay = x.EnglishDescription,
                    ArabicDisplay = x.ArabicDescription,
                }).ToList();

        }

        public async Task<List<Lookup<int>>> GetProjectTypes(string? search)
        {
            // ProjectTypes
            return (await _projectTypeRepository.GetAllAsync(search))
                .Select(x => new Lookup<int>
                {
                    Value = x.Id,
                    EnglishDisplay = x.EnglishDescription,
                    ArabicDisplay = x.ArabicDescription,
                }).ToList();

        }
    }
}
