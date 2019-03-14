using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grpc.Core;
using Grpc.Core.Utils;


using Locationproxy;
using LocationServer.Models;

namespace LocationServer
{
    public class ServiceProxy : Property.PropertyBase
    {
        private IList<LocationProperty> properties; 
        public ServiceProxy(IList<LocationProperty> props) 
        {
            properties = props;
        }
        public override Task<Detail> GetDetail(Point request, ServerCallContext context) 
        {
            return Task.FromResult(new Detail());
        }

        public override Task<Detail> GetDetailByName(DetailRequest request, ServerCallContext context) 
        {
            var props = properties.FirstOrDefault(t => t.Name.Equals(request.Name));
            if(props != null) {
                var d = new Detail{Name = props.Name, Location = new Point{Latitude = (int)props.Location.Lat, Longitude= (int)props.Location.Lon}};
                return Task.FromResult(d);
            }
            return Task.FromResult(new Detail());
        }
    }
}