using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnelidaLauncher.Model
{

    public class StatsPerformanceMonitor
    {
        private const int QUEUE_SIZE = 30;

        private Dictionary<PerformanceCounter, CircularBuffer<double>> cpuUsage;
        private Dictionary<PerformanceCounter, CircularBuffer<double>> ramUsage;



        public StatsPerformanceMonitor(List<Process> processes)
        {
            cpuUsage = new Dictionary<PerformanceCounter, CircularBuffer<double>>();

            foreach(var proc in processes)
            {
                var queue = new CircularBuffer<double>(QUEUE_SIZE);
                var name = Path.GetFileNameWithoutExtension(proc.ProcessName);
                var counter = new PerformanceCounter("Process","% Processor Time", name, true);

                cpuUsage.Add(counter, queue);
            }

            StoreCPU(new TimeSpan(0, 0, 0, 1), new CancellationToken(false));
        }

        private async Task StoreCPU(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                foreach (var entry in cpuUsage)
                {
                   entry.Value.Enqueue(Math.Round(entry.Key.NextValue() / Environment.ProcessorCount, 2));
                }
                await Task.Delay(interval, cancellationToken);
            }
        }
    }
}
