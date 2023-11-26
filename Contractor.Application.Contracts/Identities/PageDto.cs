
namespace Contractor.Identities
{
    public class PageDto
    {
        public PageDto()
        {
            Children = new List<PageDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int? ParentId { get; set; }

        public List<PageDto> Children { get; set; }
    }
}
