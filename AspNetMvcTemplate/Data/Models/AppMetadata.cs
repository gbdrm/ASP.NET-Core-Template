using System.ComponentModel.DataAnnotations;

namespace AspNetMvcTemplate.Data.Models
{
    public class AppMetadata
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }
        [MaxLength(255)]
        public string? Value { get; set; }
        [MaxLength(255)]
        public string? Type { get; set; }
        public DateTime Updated { get; set; }

        public AppMetadata()
        {
            Updated = DateTime.UtcNow.ToUniversalTime();
        }

    }
}