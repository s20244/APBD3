using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnimalsAPI.Models
{
    public class Animal
    {
        [Key]
        public int IdAnimal { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string Category { get; set; }

        [MaxLength(200)]
        public string Area { get; set; }
    }
}
