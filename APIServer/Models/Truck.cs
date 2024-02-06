using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace APIServer.Models
{
    [XmlRoot("Truck")]
    public class Truck
    {
        public int Id { get; set; }

        [XmlElement("Make")]
        [Required]
        public string Make { get; set; }

        [XmlElement("Model")]
        [Required]
        public string Model { get; set; }

        [XmlElement("Year")]
        [Required]
        [Range(1900, 2100)] // Example range, adjust as needed
        public int Year { get; set; }

        [XmlElement("Color")]
        [Required]
        public string Color { get; set; }

        public string ToXml()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Truck));
                using (var writer = new System.IO.StringWriter())
                {
                    serializer.Serialize(writer, this);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle serialization errors
                Console.WriteLine($"Error serializing Truck object: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
