using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Data;

namespace WindowsServicesMonitoring
{
    public class MainWindowViewModel
    {
        private readonly object _lock = new object();
        private ServicesManager _manager = new ServicesManager();

        public ObservableCollection<ServiceController> Services { get; private set; }
        public string Title { get; }

        public MainWindowViewModel()
        {
            Services = new ObservableCollection<ServiceController>();
            Title = "Windows Services Monitoring";

            BindingOperations.EnableCollectionSynchronization(Services, _lock);
        }

        public void Refresh()
        {
            var updatedServices = new List<ServiceController>(_manager.GetServices());

            for (int i = 0; i < Services.Count; i++)
            {
                var updatedService = updatedServices.FirstOrDefault(x => Services[i].ServiceName == x.ServiceName);
                if (updatedService != null)
                {
                    if (updatedService.Status != Services[i].Status)
                    {
                        Services[i] = updatedService;
                    }
                    updatedServices.Remove(updatedService);
                }
                else
                {
                    Services.RemoveAt(i);
                    i--;
                }
            }

            foreach (var item in updatedServices)
            {
                Services.Add(item);
            }
        }
    }
}
