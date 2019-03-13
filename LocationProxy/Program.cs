using System;

namespace LocationProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new LocationService();
            var locations = service.GetLocations(LocationService.DefaultLocationFile);
            Console.WriteLine(locations.Count);
            foreach (var item in locations)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
