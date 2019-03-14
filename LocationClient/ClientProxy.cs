using System;
using LocationProxy;
using Grpc.Core;
using System.Threading.Tasks;

namespace LocationClient
{
    public class ClientProxy
    {
        private readonly Property.PropertyClient _client;

        public ClientProxy(Property.PropertyClient client) 
        {
            _client = client;
        }

        public async Task GetDetailByName(string name) {
            try 
            {
                Log("*** GetDetail: {0}", name);
                var request = new DetailRequest{Name = name};
                var result = await _client.GetDetailByNameAsync(request);
                Log("detail found {1} with {0}", result.Name, result.Location != null);
            }
            catch (RpcException e) 
            {
                Log("RPC failed " + e);
                throw;
            }
        }

            private void Log(string s, params object[] args)
            {
                Console.WriteLine(string.Format(s, args));
            }

            private void Log(string s)
            {
                Console.WriteLine(s);
            }


    }
}

/**
public void GetFeature(int lat, int lon)
            {
                try
                {
                    Log("*** GetFeature: lat={0} lon={1}", lat, lon);

                    Point request = new Point { Latitude = lat, Longitude = lon };
                    
                    Feature feature = client.GetFeature(request);
                    if (feature.Exists())
                    {
                        Log("Found feature called \"{0}\" at {1}, {2}",
                            feature.Name, feature.Location.GetLatitude(), feature.Location.GetLongitude());
                    }
                    else
                    {
                        Log("Found no feature at {0}, {1}",
                            feature.Location.GetLatitude(), feature.Location.GetLongitude());
                    }
                }
                catch (RpcException e)
                {
                    Log("RPC failed " + e);
                    throw;
                }
            }
 */