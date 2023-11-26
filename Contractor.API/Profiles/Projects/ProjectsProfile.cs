using AutoMapper;

namespace Contractor.Projects
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
            CreateMap<ProjectTypeViewModel, ProjectTypeDto>();
            CreateMap<ProjectType, ProjectTypeDto>();

            CreateMap<DraftProjectViewModel, DraftProjectDto>();
            CreateMap<DraftProject, DraftProjectDto>();
            CreateMap<DraftProject, DraftProjectListDto>()
                .ForMember(dest => dest.ProjectOwner, opt => opt.MapFrom(src => src.Owner!.DisplayName));

            CreateMap<ProjectViewModel, ProjectDto>();
            CreateMap<Project, ProjectsListDto>()
                .ForMember(dest => dest.ProjectOwner, opt => opt.MapFrom(src => GetProjectOwner(src)))
                .ForMember(dest => dest.ProjectTypeEnglish, opt => opt.MapFrom(src => src.ProjectType!.EnglishDescription))
                .ForMember(dest => dest.ProjectTypeArabic, opt => opt.MapFrom(src => src.ProjectType!.ArabicDescription));
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectUser, ProjectUserDto>();
            CreateMap<ProjectUser, ProjectUserListDto>()
                .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.User != null ? src.User.DisplayName : ""));
            CreateMap<ProjectInvitation, ProjectInvitationDto>();
        }

        private static string? GetProjectOwner(Project project)
        {
            if (project == null)
            {
                return null; 
            }

            if(!project.ProjectUsers.Any())
            {
                return null;
            }

            var owner = project.ProjectUsers.FirstOrDefault(x => x.UserType == ProjectUserType.Owner);

            if (owner == null)
            {
                return null;
            }

            if(owner.User == null)
            {
                return null;
            }

            return owner.User.DisplayName;
        }
    }
}
