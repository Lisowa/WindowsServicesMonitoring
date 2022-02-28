using System.Collections.Generic;
using System.ServiceProcess;


namespace WindowsServicesMonitoring
{
     static class MyServiceController
    {
        public static List<Service> GetServices()
        {
            ServiceController[] S = ServiceController.GetServices();
            List<Service> Services = new List<Service>();

            foreach(var item in S)
            {
                Services.Add(new Service(item));
            }

            return Services;
        }
    }
}

