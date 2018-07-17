using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace AnnelidaLauncher.Model
{
    public class MonitoredProcess
    {
        public Process Process { get;  }
        public bool WasWarningRaised { get; set; }
        public string FriendlyName { get; }
        public string Path { get; }
        public Guid WatchDogToken { get; }

        private const int QueueSize = 20;
        private PerformanceCounter cpuUsageCounter;
        private CircularBuffer<double> cpuUsageWindow;

        private const int WatchDogTimerSeconds = 10;
        private Timer WatchDogTimer;


        public MonitoredProcess(string name, string path, bool shouldSendtoken)
        {
            Process = new Process();
            Process.StartInfo.FileName = path;
            WasWarningRaised = false;
            FriendlyName = name;
            Path = path;
            WatchDogToken = Guid.NewGuid();

            try
            {
                if(shouldSendtoken)
                    Process.StartInfo.Arguments = WatchDogToken.ToString();
                Process.Start();
                Process.EnableRaisingEvents = true;
                Process.Exited += HandleApplicationQuit;
                SetUpCounters();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{FriendlyName} não econtrado.", "Erro", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                return;
            }

            if (!shouldSendtoken) return;
            WatchDogTimer = new Timer(WatchDogTimerSeconds * 1000);
            WatchDogTimer.Elapsed += WatchDogTimerElapsed;
        }

        private void WatchDogTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("timer elapsed");
        }

        private void SetUpCounters()
        {
            cpuUsageWindow = new CircularBuffer<double>(QueueSize);
            var realProcName = System.IO.Path.GetFileNameWithoutExtension(Process.ProcessName);
            cpuUsageCounter = new PerformanceCounter("Process", "% Processor Time", realProcName, true);
        }

        public void EnqueueCpuValue()
        {
            cpuUsageWindow.Enqueue(Math.Round(cpuUsageCounter.NextValue() / Environment.ProcessorCount, 2));
        }

        public bool TryGetSD(out double sd)
        {
            sd = -1;
            if (cpuUsageWindow.Count != QueueSize)
                return false;
            sd = CalculateStdDev(cpuUsageWindow);
            return true;
        }

        public void ResetTimer()
        {
            Console.WriteLine($"Timer for {FriendlyName} refreshed");
            WatchDogTimer.Interval = WatchDogTimerSeconds * 1000;
        }

        private double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }

        private void HandleApplicationQuit(object sender, EventArgs args)
        {
            MessageBox.Show($"A aplicação {FriendlyName} fechou! Abrindo novamente", "Alerta!", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            Process.Start();
            SetUpCounters();
        }
    }
}
