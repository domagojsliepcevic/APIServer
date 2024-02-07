using SOAPServer.Interfaces;
using SOAPServer.Models;
using SOAPServer.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
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
        public XmlDocument SearchXmlFile(string xpathExpression)
        {
            // Get the physical path of the XML file in the project root
            string fileName = "searchTerm.xml";
            string filePath = HttpContext.Current.Server.MapPath($"~/{fileName}");

            // Load the XML file
            XDocument xmlDocument = XDocument.Load(filePath);

            // Execute the XPath expression and retrieve the results
            var results = xmlDocument.XPathSelectElements(xpathExpression);

            // Construct the XML document for the array of trucks
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement trucksElement = xmlDoc.CreateElement("Trucks");
            foreach (var element in results)
            {
                XmlElement truckElement = xmlDoc.CreateElement("Truck");
                truckElement.AppendChild(CreateXmlElement(xmlDoc, "Make", element.Element("Make").Value));
                truckElement.AppendChild(CreateXmlElement(xmlDoc, "Model", element.Element("Model").Value));
                truckElement.AppendChild(CreateXmlElement(xmlDoc, "Year", element.Element("Year").Value));
                truckElement.AppendChild(CreateXmlElement(xmlDoc, "Color", element.Element("Color").Value));
                trucksElement.AppendChild(truckElement);
            }
            xmlDoc.AppendChild(trucksElement);

            // Return the XML document
            return xmlDoc;
        }

        // Helper method to create an XML element
        private XmlElement CreateXmlElement(XmlDocument xmlDoc, string elementName, string value)
        {
            XmlElement xmlElement = xmlDoc.CreateElement(elementName);
            xmlElement.InnerText = value;
            return xmlElement;
        }


    }
}
