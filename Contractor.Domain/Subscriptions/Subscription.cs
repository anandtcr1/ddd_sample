using Contractor.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Subscriptions
{
    public class Subscription
    {
        private Subscription(string name, float storageSpace)
        {
            Name = name;
            ProjectFolderTemplates = new HashSet<ProjectFolderTemplate>();
            StorageSpace = storageSpace;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; } = null!;

        public float StorageSpace { get; private set; }


        public virtual ICollection<ProjectFolderTemplate>? ProjectFolderTemplates { get; private set; }


        public static Subscription Create(string name, float storageSpace)
        {
            return new Subscription(name, storageSpace);
        }

        public void AddProjectFolderTemplate(string folderName)
        {
            ProjectFolderTemplates.Add(ProjectFolderTemplate.Create(folderName,Id));
        }

        public void Update(string name, float storageSpace)
        {
            Name = name;
            StorageSpace = storageSpace;
        }

    }
}
