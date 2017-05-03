using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using WindowsDesktop;

namespace VDesk {
    partial class MainWindow {
        public ObservableCollection<VirtualDesktop> desktops;

        public MainWindow() {
            desktops = new ObservableCollection<VirtualDesktop>(VirtualDesktop.GetDesktops());
            VirtualDesktop.Created += VirtualDesktop_Created;
            VirtualDesktop.Destroyed += VirtualDesktop_Destroyed;
            InitializeComponent();
            view.DataContext = this;
            lstDesktops.ItemsSource = desktops;
        }

        private void VirtualDesktop_Destroyed(object sender, VirtualDesktopDestroyEventArgs e) {
            desktops.Remove(e.Destroyed);
        }

        private void VirtualDesktop_Created(object sender, VirtualDesktop e) {
            desktops.Add(e);
        }

        private void CreateNew(object sender, RoutedEventArgs e) {
            VirtualDesktop.Create().Switch();
        }
        
    }
}
