using System;
using System.Management;
using System.ServiceProcess;


namespace WindowsServicesMonitoring
{
    public class Service
    {
        public string Name { get; }
        public string DisplayName { get; }
        public string Status { get; }
        public string Account { get; }


        public bool CanStart
        {
            get
            {
                return Status != ServiceControllerStatus.Running.ToString();
            }
        }
        public bool CanStop
        {
            get
            {
                using (var srv = new ServiceController(Name))
                {
                    return srv.CanStop;
                }
            }
        }

        public Service(ServiceController item)
        {
            Name = item.ServiceName;
            DisplayName = item.DisplayName;
            Status = item.Status.ToString();
            Account = SetAccount(Name);
        }
        public void Start()
        {
            var ex = new ServiceController(Name);
            ex.Start();
        }
        public void Stop()
        {
            new ServiceController(Name).Stop();
        }


        private string SetAccount(string serviceName)
        {
            string objPath = $"Win32_Service.Name='{serviceName}'";
            using (ManagementObject service = new ManagementObject(new ManagementPath(objPath)))
            {
                var value = service.Properties["StartName"].Value;
                return (value != null) ? value.ToString() : string.Empty;
            }
        }
    }
}

