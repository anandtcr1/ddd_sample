using System.ComponentModel.DataAnnotations;

namespace Contractor.Files
{
    public class File
    {
        private File(string name, float size, string token, FileType fileType, string path, string extension)
        {
            Name = name;
            Size = size;
            Token = token;
            FileType = fileType;
            Path = path;
            Extension = extension;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; } = null!;

        [Required]
        public string Path { get; private set; } = null!;

        [Required]
        public string Extension { get; private set; } = null!;

        public float Size { get; private set; }

        [Required]
        public string Token { get; private set; } = null!;

        public FileType FileType { get; private set; }

        
        public static File Create(string name, float size, FileType fileType, string path, string extension)
        {
            var token = Guid.NewGuid().ToString();
            path = path + "/" + token + name;
            return new File(name, size, token, fileType, path, extension);
        }
    }
}
