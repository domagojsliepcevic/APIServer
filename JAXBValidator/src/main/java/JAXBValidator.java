import generated.Trucks;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Unmarshaller;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.List;

public class JAXBValidator {
    public static void main(String[] args) {

        // Step 1: Read SOAP response XML
        File soapResponseXmlFile = new File("D:\\Shared Folder\\IIS\\Project\\APIServer\\SOAPServer\\response.xml");



        try {
            // Step 2: Deserialize XML into Java objects
            JAXBContext jaxbContext = JAXBContext.newInstance(Trucks.class);
            Unmarshaller unmarshaller = jaxbContext.createUnmarshaller();
            Trucks trucks = (Trucks) unmarshaller.unmarshal(soapResponseXmlFile);

            // Step 3: Validate the Java objects
            boolean isValidated = validateTrucks(trucks);

            // Step 4: Write validation result to a text file
            writeValidationResultToFile(isValidated);
        } catch (JAXBException | IOException e) {
            e.printStackTrace();
        }
    }

    private static boolean validateTrucks(Trucks trucks) {
        List<Trucks.Truck> truckList = trucks.getTruck();

        // Iterate through each Truck object
        for (Trucks.Truck truck : truckList) {
            // Validate Make, Model, Year, and Color
            if (isEmpty(truck.getMake()) || isEmpty(truck.getModel()) || !isPositiveInteger(String.valueOf(truck.getYear())) || isEmpty(truck.getColor())) {
                return false;
            }
        }
        return true;
    }

    private static boolean isPositiveInteger(String value) {
        try {
            int year = Integer.parseInt(value);
            return year > 0;
        } catch (NumberFormatException e) {
            // Parsing failed, not a valid integer
            return false;
        }
    }


    private static boolean isEmpty(String value) {
        return value == null || value.trim().isEmpty();
    }


    private static void writeValidationResultToFile(boolean isValidated) throws IOException {
        File file = new File("validationResult.txt");
        try (FileWriter writer = new FileWriter(file)) {
            writer.write(isValidated ? "Validated" : "Not Validated");
            System.out.println("File path: " + file.getAbsolutePath());
        }
    }
}
