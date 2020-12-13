using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceProcess;
using System.ComponentModel;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ToggleDatabaseServices
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> dbServiceDisplayNames = new List<string>(new string[] { "MySQL80" });
            List<ServiceController> scs = ServiceController.GetServices().ToList<ServiceController>();
            foreach (ServiceController sc in scs)
            {
                bool serviceStartTypeToggled = false;
                if (!string.IsNullOrWhiteSpace(dbServiceDisplayNames.Find((x) => x == sc.DisplayName)))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        if (sc.StartType == ServiceStartMode.Disabled)
                        {
                            if (ToggleServiceStartType())
                                continue;
                            serviceStartTypeToggled = true;
                        }
                        sc.Start();
                    }
                    else if (sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                        if(serviceStartTypeToggled)
                        {
                            ToggleServiceStartType();
                        }
                    }
                    listBox1.Items.Add($"{sc.DisplayName} is {sc.Status}");
                }
            }
        }

        private bool ToggleServiceStartType()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\MySQL80"))
                {
                    if (Convert.ToInt32(regKey.GetValue("Start").ToString()) == 4)
                    {
                        regKey.SetValue("Start", 3);
                    }
                    else
                    {
                        regKey.SetValue("Start", 4);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
