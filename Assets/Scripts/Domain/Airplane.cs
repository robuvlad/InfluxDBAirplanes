using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;

namespace Assets.Scripts.Domain
{
    public class Airplane
    {
        [InfluxTimestamp]
        public DateTime Timestamp { get; set; }

        [InfluxField("Id")]
        public int Id { get; set; }

        [InfluxField("Name")]
        public string Name { get; set; }

        [InfluxField("Start_Location")]
        public string StartLocation { get; set; }

        [InfluxField("End_Location")]
        public string EndLocation { get; set; }

        [InfluxField("Speed")]
        public int Speed { get; set; }

        [InfluxField("Altitude")]
        public int Altitude { get; set; }

    }
}
