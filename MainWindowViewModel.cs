using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
//using System.ServiceProcess;
using System.Windows.Data;

namespace WindowsServicesMonitoring
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Service selectedService;
        private readonly object _lock = new object();

        public ObservableCollection<Service> Services { get; private set; }
        public Service SelectedService
        {
            get { return selectedService; }
            set
            {
                selectedService = value;
                OnPropertyChanged(nameof(CanStart));
                OnPropertyChanged(nameof(CanStop));
            }
        }

        public bool CanStart
        {
            get
            {
                if (IsBusy) return false;

                if (SelectedService != null && SelectedService.CanStart)
                    return true;
                return false;
            }
        }
        public bool CanStop
        {
            get
            {
                if (IsBusy) return false;

                if (SelectedService != null && SelectedService.CanStop)
                    return true;
                return false;
            }
        }

        public bool IsBusy { get; set; }

        public MainWindowViewModel()
        {
            Services = new ObservableCollection<Service>();

            BindingOperations.EnableCollectionSynchronization(Services, _lock);
        }

        public void Refresh()
        {
            List<Service> updatedServices = MyServiceController.GetServices();

            for (int i = 0; i < Services.Count; i++)
            {
                var updatedService = updatedServices.FirstOrDefault(x => Services[i].Name == x.Name);
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

        internal void Stop()
        {
            OnPropertyChanged(nameof(CanStop));
            SelectedService.Stop();
        }

        internal void Start()
        {
            OnPropertyChanged(nameof(CanStart));
            SelectedService.Start();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
