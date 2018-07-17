using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AnnelidaLauncher.Model
{

    public class StatsPerformanceMonitor
    {
        private const double LOW_SD_THRES = 0.01;
        private const double UPPER_SD_THRES = 3;
        private readonly ICollection<MonitoredProcess> monitoredProcesses;
        
        public StatsPerformanceMonitor(ICollection<MonitoredProcess> monitoredProcesses)
        {
            this.monitoredProcesses = monitoredProcesses;
            UpdateCPU(new TimeSpan(0, 0, 0, 0,500), new CancellationToken(false));
        }

        private async Task UpdateCPU(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var entry in monitoredProcesses)
                {
                    entry.EnqueueCpuValue();
                    if (!entry.TryGetSD(out var sd)) continue;
                    if (sd > UPPER_SD_THRES && !entry.WasWarningRaised)
                    {
                        entry.WasWarningRaised = true;
                        MessageBox.Show(
                            $"A applicação {entry.FriendlyName} está se comportando fora do esperado. CPU SD={sd}",
                            "Alerta!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if( sd < UPPER_SD_THRES && entry.WasWarningRaised)
                    {
                        entry.WasWarningRaised = false;
                    }

                    Console.WriteLine(sd);
                }
                await Task.Delay(interval, cancellationToken);
            }
        }

        private void PrintAverage(IEnumerable<double> buffer)
        {
            double avg = 0;
            foreach (var entry in buffer)
            {
                avg += entry;
            }

            avg = avg / buffer.Count();

            Console.WriteLine(avg);
        }
     
    }
}
