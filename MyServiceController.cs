using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;


namespace WindowsServicesMonitoring
{
    public static class ServiceManager
    {
        public static List<Service> GetServices()
        {
            ServiceController[] serviceControllers = ServiceController.GetServices();
            List<Service> services = serviceControllers.Select(x => new Service(x)).ToList();

            foreach (var item in serviceControllers)
                item.Dispose();

            return services;
        }
    }
}

