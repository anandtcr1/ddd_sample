using AutoMapper;

namespace Contractor.Contracts
{
    public class ContractsProfile: Profile
    {
        public ContractsProfile()
        {
            CreateMap<ContractTypeViewModel, ContractTypeDto>();
            CreateMap<ContractType, ContractTypeDto>();

        }
    }
}
