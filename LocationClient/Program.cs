using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("127.0.0.1:9222", ChannelCredentials.Insecure);
            var client = new ClientProxy(new Locationproxy.Property.PropertyClient(channel));

            // ACME
            client.GetDetailByName("ACME").Wait();
            //Bernier Group
            client.GetDetailByName("Bernier Group").Wait();

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
