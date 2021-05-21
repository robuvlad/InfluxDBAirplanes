using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;

namespace Assets.Scripts.Domain
{
    public class AirplanePosition
    {
        [InfluxTimestamp]
        public DateTime Timestamp { get; set; }

        [InfluxField("Id")]
        public int Id { get; set; }

        [InfluxField("Name")]
        public string Name { get; set; }

        [InfluxField("X_Position")]
        public float X_Position{ get; set; }

        [InfluxField("Y_Position")]
        public float Y_Position { get; set; }
    }
}
