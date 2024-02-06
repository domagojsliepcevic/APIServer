// Car.cs
using System.ComponentModel.DataAnnotations;

namespace APIServer.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string? Make { get; set; }

        [Required]
        public string? Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string? Color { get; set; }

        // XML data property
        [Required]
        public string? XmlData { get; set; }
    }
}
