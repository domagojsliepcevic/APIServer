using APIServer.Interface;
using APIServer.Models;
using System;
using System.IO;
using System.Xml;
using Commons.Xml.Relaxng;

namespace APIServer.Services
{
    public class CustomValidator : ICustomValidator
    {
        private readonly string rngFilePath; // Path to RNG schema file

        public CustomValidator(string rngFilePath)
        {
            this.rngFilePath = rngFilePath;
        }

        public bool ValidateTruck(Truck truck)
        {
            try
            {
                var xml = truck.ToXml(); // Convert Truck object to XML

                // Load the RelaxNG schema file
                var schema = RelaxngPattern.Read(new XmlTextReader(rngFilePath));

                // Create a RelaxngValidatingReader for the truck XML
                using (var stringReader = new StringReader(xml))
                using (var xmlReader = XmlReader.Create(stringReader))
                using (var validatingReader = new RelaxngValidatingReader(xmlReader, schema))
                {
                    // Validate the truck XML against the RelaxNG schema
                    while (validatingReader.Read()) { }
                }

                return true; // Validation passed
            }
            catch (RelaxngException ex)
            {
                Console.WriteLine($"Error validating Truck object: {ex.Message}");
                return false; // Validation failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false; // Validation failed
            }
        }
    }
}
