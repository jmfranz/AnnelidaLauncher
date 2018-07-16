using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AnnelidaLauncher.Model
{
    class Launcher
    {
        private readonly List<Process> annelidaProcesses;
        private StatsPerformanceMonitor statsPerformanceMonitor;

        public ConfigurationFile ConfigurationFile { get; private set; }
        public Launcher()
        {
            annelidaProcesses = new List<Process>();

            if (LoadConfigFile())
            {
                LaunchApplications(ConfigurationFile);
                statsPerformanceMonitor = new StatsPerformanceMonitor(annelidaProcesses);
            }
        }

        private bool LoadConfigFile()
        {
            try
            {
                using (StreamReader file = File.OpenText(@"config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    ConfigurationFile = (ConfigurationFile)serializer.Deserialize(file, typeof(ConfigurationFile));
                }

                return true;
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Arquivo de configuração (config.json) não encontrado.","Erro", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }

        private void LaunchApplications(ConfigurationFile config)
        {
            Process mongo = new Process();
            mongo.StartInfo.FileName = config.MongoAddress;
            var current = Process.GetProcesses();
            mongo.Start();
            mongo.EnableRaisingEvents = true;
            mongo.Exited += (sender, args) => { Console.WriteLine("mongo quitted"); };
            annelidaProcesses.Add(mongo);


            //Process dispacher = new Process();
            //dispacher.StartInfo.FileName = config.DispatcherAddress;
            //try
            //{
            //    dispacher.Start();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Dispatcher não econtrado.", "Erro", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return;
            //}
            //annelidaProcesses.Add(dispacher);

            //Process annelida3D = new Process();
            //annelida3D.StartInfo.FileName = config.Annelida3DAddress;
            //try
            //{
            //    annelida3D.Start();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Annelida 3D não econtrado.", "Erro", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return;
            //}
            //annelidaProcesses.Add(annelida3D);

            //Process annelida2D = new Process();
            //annelida2D.StartInfo.FileName = config.Annelida2DAddress;
            //try
            //{
            //    annelida2D.Start();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Annelida 32D não econtrado.", "Erro", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
            //annelidaProcesses.Add(annelida2D);
        }
    }
}
