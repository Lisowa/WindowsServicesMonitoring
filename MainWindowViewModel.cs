﻿using System;
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
        public string Title { get; }
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
                if (SelectedService != null && SelectedService.CanStart)
                    return true;
                return false;
            }
        }
        public bool CanStop
        {
            get
            {
                if (SelectedService != null && SelectedService.CanStop)
                    return true;
                return false;
            }
        }

        public MainWindowViewModel()
        {
            Services = new ObservableCollection<Service>();
            Title = "Windows Services Monitoring";

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
            SelectedService.Stop();
        }

        internal void Start()
        {
            SelectedService.Start();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
