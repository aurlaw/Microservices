using System;
using LocationProxy;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new LocationService();
            var locations = service.GetLocations(LocationService.DefaultLocationFile);
            const int Port = 52020;

            var server = new Server
            {
                Services = {Locationproxy.Property.BindService(new ServiceProxy(locations))},
                Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)}
            };
            server.Start();
            Console.WriteLine($"Location Service started and listening on port {Port}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
            // Console.WriteLine(locations.Count);
            // foreach (var item in locations)
            // {
            //     Console.WriteLine(item.Name);
            // }
        }
    }
}
