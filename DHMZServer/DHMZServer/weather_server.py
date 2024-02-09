from xmlrpc.server import SimpleXMLRPCServer
import requests
from xml.etree import ElementTree as ET

# Define a function to fetch temperature data from DHMZ
def fetch_temperature(city_name):
    # URL for DHMZ data
    url = "https://vrijeme.hr/hrvatska_n.xml"
    
    # Send a request to DHMZ and get the XML response
    response = requests.get(url)
    xml_data = response.text
    
    # Parse the XML response and extract temperature for the specified city
    root = ET.fromstring(xml_data)
    for grad in root.findall("./Grad"):
        grad_ime = grad.find("GradIme").text.strip()
        if grad_ime == city_name:
            temperature = grad.find("Podatci/Temp").text.strip()
            return float(temperature)
    
    # If city name is not found in the XML data, return None
    return None

# Create an XML-RPC server
server = SimpleXMLRPCServer(("localhost", 8000))
print("XML-RPC server is running on port 8000...")

# Register the "fetch_temperature" function
server.register_function(fetch_temperature, "fetch_temperature")

try:
    # Start the server
    server.serve_forever()
except KeyboardInterrupt:
    # Catch KeyboardInterrupt (Ctrl+C) and print a message
    print("Shutting down the server...")
    
    # Clean up resources, if any
    # For example, you might want to close any open files or database connections here
    # In this example, there are no specific cleanup actions to perform
    
    # Shutdown the server gracefully
    server.shutdown()
