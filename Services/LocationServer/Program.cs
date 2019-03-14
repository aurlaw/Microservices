using System;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Threading;

namespace LocationServer
{
    class Program
    {
        const string LOCATION_SERVICE_ADDRESS = "LISTEN_ADDR";
        const string LOCATION_SERVICE_PORT = "PORT";

        [Verb("start", HelpText = "Starts the server listening on provided port")]
        class ServerOptions
        {
            [Option('h', "hostname", HelpText = "The ip on which the server is running. If not provided, LISTEN_ADDR environment variable value will be used. If not defined, localhost is used")]
            public string Host { get; set; }

            [Option('p', "port", HelpText = "The port on for running the server")]
            public int Port { get; set; }

        }

        static object StartServer(string host, int port)
        {
            // Run the server in a separate thread and make the main thread busy waiting.
            // The busy wait is because when we run in a container, we can't use techniques such as waiting on user input (Console.Readline())
            Task serverTask = Task.Run(async () =>
            {
                try
                {
                    var service = new LocationService();
                    var locations = service.GetLocations(LocationService.DefaultLocationFile);
                    Console.WriteLine($"Trying to start a grpc server at  {host}:{port}");

                    var server = new Server
                    {
                        Services = {Locationproxy.Property.BindService(new ServiceProxy(locations))},
                        Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
                    };
                    Console.WriteLine($"Location server is listening at {host}:{port}");
                    server.Start();
                    Console.WriteLine("Initialization completed");

                    // Keep the server up and running
                    while(true)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(10));
                    }
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
            switch (args[0])
            {
                case "start":
                    Parser.Default.ParseArguments<ServerOptions>(args).MapResult(
                        (ServerOptions options) => 
                        {
                            Console.WriteLine($"Started as process with id {System.Diagnostics.Process.GetCurrentProcess().Id}");

                            // Set hostname/ip address
                            string hostname = options.Host;
                            if (string.IsNullOrEmpty(hostname))
                            {
                                Console.WriteLine($"Reading host address from {LOCATION_SERVICE_ADDRESS} environment variable");
                                hostname = Environment.GetEnvironmentVariable(LOCATION_SERVICE_ADDRESS);
                                if (string.IsNullOrEmpty(hostname))
                                {
                                    Console.WriteLine($"Environment variable {LOCATION_SERVICE_ADDRESS} was not set. Setting the host to 0.0.0.0");
                                    hostname = "0.0.0.0";
                                }
                            }

                            // Set the port
                            int port = options.Port;
                            if (options.Port <= 0)
                            {
                                Console.WriteLine($"Reading cart service port from {LOCATION_SERVICE_PORT} environment variable");
                                string portStr = Environment.GetEnvironmentVariable(LOCATION_SERVICE_PORT);
                                if (string.IsNullOrEmpty(portStr))
                                {
                                    Console.WriteLine($"{LOCATION_SERVICE_PORT} environment variable was not set. Setting the port to 9222");
                                    port = 9222;
                                }
                                else    
                                {
                                    port = int.Parse(portStr);
                                }
                            }
                            return StartServer(hostname, port);
                        },
                        errs => 1);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}
