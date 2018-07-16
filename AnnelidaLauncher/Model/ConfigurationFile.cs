using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnelidaLauncher.Model
{
    class ConfigurationFile
    {
        public string MongoAddress { get; set; }
        public string DispatcherAddress { get; set; }
        public string Annelida3DAddress { get; set; }
        public string Annelida2DAddress { get; set; }

        public ConfigurationFile(string mongo, string dispatcher, string annelida2D, string annelida3D)
        {
            MongoAddress = mongo;
            DispatcherAddress = dispatcher;
            Annelida3DAddress = annelida3D;
            Annelida2DAddress = annelida2D;
        }
    }
}
