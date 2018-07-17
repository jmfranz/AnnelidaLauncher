using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnelidaLauncher.Model
{
    class WatchDogListener
    {
        private UdpClient watchdogListener;
        private CancellationTokenSource cts;
        private Dictionary<Guid,MonitoredProcess> monitoredProcessesDict;

        public WatchDogListener(int port, ICollection<MonitoredProcess> monitoredProcesses)
        {
            monitoredProcessesDict = new Dictionary<Guid, MonitoredProcess>();
            foreach (var monitoredProcess in monitoredProcesses)
            {
                monitoredProcessesDict.Add(monitoredProcess.WatchDogToken,monitoredProcess);
            }
            cts = new CancellationTokenSource();
            //TODO: Fix broadcaster to local machine addr + 255
            var endpoint = new IPEndPoint(IPAddress.Any, port );
            watchdogListener = new UdpClient();
            watchdogListener.EnableBroadcast = true;
            watchdogListener.Client.Bind(endpoint);
            ListenToApplicationBeacons();
        }

        private async Task ListenToApplicationBeacons()
        {
            while (!cts.IsCancellationRequested)
            {
                var readTask = await watchdogListener.ReceiveAsync();

                var recvGuid = new Guid(readTask.Buffer);
                if (monitoredProcessesDict.ContainsKey(recvGuid))
                {
                    monitoredProcessesDict[recvGuid].ResetTimer();
                }
            }
        }
    }
}
