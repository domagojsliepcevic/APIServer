import xmlrpc.client

# Create an XML-RPC proxy to connect to the server
proxy = xmlrpc.client.ServerProxy("http://localhost:8000/")

def fetch_temperature():
    try:
        while True:
            # Prompt the user to enter the city name or 'exit' to quit
            city_name = input("Enter the name of the city to fetch temperature for (or 'exit' to quit): ")
            
            if city_name.lower() == 'exit':
                print("Exiting the program.")
                break
            
            # Call the remote function on the server to fetch temperature data
            temperature = proxy.fetch_temperature(city_name)
            if temperature is not None:
                print(f"The temperature in {city_name} is: {temperature}°C")
            else:
                print(f"Temperature data for {city_name} not found.")
    except Exception as e:
        print(f"An error occurred: {e}")

# Fetch temperature for a specific city entered by the user
fetch_temperature()
