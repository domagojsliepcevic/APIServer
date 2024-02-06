using SOAPServer.Interfaces;
using SOAPServer.Models;
using SOAPServer.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SOAPServer
{
    /// <summary>
    /// Summary description for TruckService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CarService : System.Web.Services.WebService
    {
        private readonly ITruckRepository _truckRepository;

        public CarService()
        {

            _truckRepository = new TruckRepository(new Data.TrucksContext());
        }

        [WebMethod]
        public string GenerateXmlFile(string searchTerm)
        {
            // Retrieve trucks that match the search term
            var cars = _truckRepository.GetTrucksBySearchTerm(searchTerm);

            // Create XML document containing car information
            var xml = new XDocument(
                new XElement("Trucks",
                    cars.Select(car => new XElement("Truck",
                        new XElement("Make", car.Make),
                        new XElement("Model", car.Model),
                        new XElement("Year", car.Year),
                        new XElement("Color", car.Color)
                    ))
                )
            );

            // Save the XML document to a file
            string fileName = "searchTerm.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            xml.Save(filePath);

            return filePath; // Return the path to the generated XML file
        }

        [WebMethod]
        public string SearchXmlFile(string xpathExpression)
        {
            // Load the XML file
            string fileName = "searchTerm.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            XDocument xmlDocument = XDocument.Load(filePath);

            // Execute the XPath expression and iterate over the results
            var results = xmlDocument.XPathSelectElements(xpathExpression);

            // Convert the XML content to string
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var element in results)
            {
                resultBuilder.AppendLine(element.ToString(SaveOptions.DisableFormatting)); // Disable formatting to remove extra whitespaces
            }
            string resultString = resultBuilder.ToString().Trim(); // Trim leading and trailing whitespaces

            // Return the XML content as a string
            return resultString;

        }

    }
}
