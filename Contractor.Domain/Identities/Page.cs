
namespace Contractor.Identities
{
    public class Page
    {
        private Page(string name, int? parentId)
        {
            Name = name;
            ParentId = parentId;
            Children = new HashSet<Page>();
            IsActive = true;
        }

        public int Id { get; private set; }

        public string Name { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public int? ParentId { get; private set; }


        public virtual Page? Parent { get; private set; }

        public virtual ICollection<Page> Children { get; private set; }

        public static Page Create(string name, int? parentId)
        {
            return new Page(name, parentId);
        }

        public static List<Page> Create(List<string> names, int? parentId)
        {
            List<Page> pages = new();

            foreach (string name in names)
            {
                pages.Add(Create(name, parentId));
            }

            return pages;
        }
    }
}
