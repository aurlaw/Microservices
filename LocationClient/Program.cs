using CommandLine;
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

        [Verb("start", HelpText = "Starts the client connecting on provided port")]
        class ServerOptions
        {
            [Option('h', "hostname",  HelpText = "The ip on which the server is running. If not provided, LISTEN_ADDR environment variable value will be used. If not defined, localhost is used")]
            public string Host { get; set; }

            [Option('p', "port", Required=true, HelpText = "The port on for running the server")]
            public int Port { get; set; }

        }
         static object Connect(string host, int port)
        {
            // Run the server in a separate thread and make the main thread busy waiting.
            // The busy wait is because when we run in a container, we can't use techniques such as waiting on user input (Console.Readline())
            Task serverTask = Task.Run(async () =>
            {
                try
                {
                    var channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
                    var client = new ClientProxy(new LocationProxy.Property.PropertyClient(channel));

                    // ACME
                    client.GetDetailByName("ACME").Wait();
                    //Bernier Group
                    client.GetDetailByName("Bernier Group").Wait();

                    await channel.ShutdownAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            return Task.WaitAny(new[] { serverTask });
        }
        static void Main(string[] args)
        {
             if (args.Length == 0)
            {
                Console.WriteLine("Invalid number of arguments supplied");
                Environment.Exit(-1);
            }   
            Parser.Default.ParseArguments<ServerOptions>(args).MapResult(
                        (ServerOptions options) => 
                        {
                            // Set hostname/ip address
                            string hostname = options.Host;
                            if (string.IsNullOrEmpty(hostname))
                            {
                                Console.WriteLine($"-hostname was not provided. Setting the host to 0.0.0.0");
                                hostname = "0.0.0.0";
                            }
                            // Set the port
                            int port = options.Port;
      

                            return Connect(hostname, port);                     
                        },
                        errs => 1);
        }
    }
}
