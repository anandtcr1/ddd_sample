using Contractor.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Lookups
{
    public interface ILookupAppService
    {
        Task<List<Lookup<string>>> GetRoles(string search);

        Task<List<Lookup<int>>> GetProjects(string userId, string? search);
        
        Task<List<Lookup<int>>> GetCities(string? search);

        Task<List<Lookup<int>>> GetAreas(int cityId, string? search);

        Task<List<Lookup<int>>> GetNationalities(string? search);

        Task<List<Lookup<int>>> GetContractTypes(string? search);

        Task<List<Lookup<int>>> GetOutGoingTypes(string? search);

        Task<List<Lookup<int>>> GetIncomeTypes(string? search);
        Task<List<Lookup<int>>> GetProjectTypes(string? search); 
    }
}
