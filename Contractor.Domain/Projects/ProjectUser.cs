using Contractor.Identities;

namespace Contractor.Projects
{
    public class ProjectUser
    {
        private ProjectUser(bool canSeeTenders, bool canAddUsers, bool canEditProject, ProjectUserType userType, int projectId, string userId)
        {
            CanSeeTenders = canSeeTenders;
            CanAddUsers = canAddUsers;
            CanEditProject = canEditProject;
            UserType = userType;
            ProjectId = projectId;
            UserId = userId;
        }

        public int Id { get; private set; }

        public bool CanSeeTenders { get; private set; }

        public bool CanAddUsers { get; private set; }

        public bool CanEditProject { get; private set; }

        public ProjectUserType UserType { get; private set; }

        public int ProjectId { get; private set; }

        public string UserId { get; private set; } = null!;



        public virtual Project Project { get; private set; }

        public virtual User User { get; private set; }


        public static ProjectUser Create(bool canSeeTenders, bool canAddUsers, bool canEditProject, ProjectUserType userType, int projectId, string userId) 
        {
            return new ProjectUser(canSeeTenders, canAddUsers, canEditProject, userType, projectId, userId);
        }
    }
}
