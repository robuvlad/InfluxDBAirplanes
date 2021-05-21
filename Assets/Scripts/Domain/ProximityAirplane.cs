using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;

namespace Assets.Scripts.Domain
{
    public class ProximityAirplane
    {
        [InfluxTimestamp]
        public DateTime Timestamp { get; set; }

        [InfluxField("BaseAirplaneName")]
        public string BaseAirplaneName { get; set; }

        [InfluxField("OtherAirplaneName")]
        public string OtherAirplaneName { get; set; }

        [InfluxField("Distance")]
        public float Distance { get; set; }
    }
}
