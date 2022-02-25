using System.ServiceProcess;


namespace WindowsServicesMonitoring
{
    class ServicesManager
    {
        public ServiceController[] GetServices()
        {
            return ServiceController.GetServices();
        }
    }
}

