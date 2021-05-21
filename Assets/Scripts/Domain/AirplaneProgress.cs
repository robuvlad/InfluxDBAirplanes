using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;

namespace Assets.Scripts.Domain
{
    public class AirplaneProgress
    {
        [InfluxTimestamp]
        public DateTime Timestamp { get; set; }

        [InfluxField("Id")]
        public int Id { get; set; }

        [InfluxField("Name")]
        public string Name { get; set; }

        [InfluxField("Progress")]
        public int Progress { get; set; }
    }
}
