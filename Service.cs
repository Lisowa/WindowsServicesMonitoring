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
        private ServiceController serviceController;

        public bool CanStart
        {
            get
            {
                if (serviceController.Status != ServiceControllerStatus.Running)
                    return true;
                return false;
            }
        }
        public bool CanStop
        {
            get
            {
                if (serviceController.CanStop)
                    return true;
                return false;
            }
        }


        public Service(ServiceController item)
        {
            Name = item.ServiceName;
            DisplayName = item.DisplayName;
            Status = item.Status.ToString();
            Account = SetAccount(Name);
            serviceController = item;
        }
        public void Start()
        {
            serviceController.Start();
            serviceController.WaitForStatus(ServiceControllerStatus.Running);
        }
        public void Stop()
        {
            serviceController.Stop();
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

