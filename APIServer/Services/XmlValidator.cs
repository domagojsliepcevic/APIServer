// XmlValidator.cs
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace APIServer.Services
{
    public class XmlValidator
    {
        private readonly string _xsdFilePath;

        public XmlValidator(string xsdFilePath)
        {
            _xsdFilePath = xsdFilePath;
        }

        public bool ValidateXml(string xmlData)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", _xsdFilePath);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemas;

            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlData), settings))
                {
                    while (reader.Read()) { }
                }
                return true;
            }
            catch (XmlException)
            {
                return false;
            }
        }
    }
}
