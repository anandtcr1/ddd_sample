using AutoMapper;
using Contractor.ViewModels.Files;

namespace Contractor.Files
{
    public class FileProfile: Profile
    {
        public FileProfile()
        {
            CreateMap<AccessDefinitionViewModel, FileDto>();
            CreateMap<File, FileDto>();
            CreateMap<AccessDefinition, AccessDefinitionDto>();
        }
    }
}
