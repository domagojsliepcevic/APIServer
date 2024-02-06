

using APIServer.Models;

namespace APIServer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            // Look for any cars.
            if (context.Cars.Any())
            {
                return;   // DB has been seeded
            }

            var cars = new Car[]
            {
                new Car{
                    Make="Hyundai",
                    Model="Accent",
                    Year = 2000,
                    Color="Silver",
                    XmlData = "<car><make>Hyundai</make><model>Accent</model><year>2000</year><color>Silver</color></car>"
                }
               
            };

            context.Cars.AddRange(cars);
            context.SaveChanges();


            var trucks = new Truck[]
            {
                    new Truck
                    {
                        Make = "Ford",
                        Model = "F-150",
                        Year = 2022,
                        Color = "Red"
       
                    }
            };

            context.Trucks.AddRange(trucks);
            context.SaveChanges();


          

        }
    }
}