using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vibrant.InfluxDB.Client;

namespace Assets.Scripts
{
    public static class InfluxTest
    {
        public static async Task Main()
        {
            Debug.Log("Main");

            var client = new InfluxClient(new Uri("http://localhost:8086"));

            int rows = 100;
            var start = DateTime.UtcNow;
            int sleepTime = 5000;
            //var start = new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var timestamp = start;
            for (int i = 0; i < rows; i++)
            {
                //var infos = CreateTypedRowsStartingAt(new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc), 50);

                ComputerInfo[] info = CreateTypedRowStartingAt(timestamp);
                await client.WriteAsync("mydb", "comp", info);
                timestamp = timestamp.AddMilliseconds(sleepTime);

                System.Threading.Thread.Sleep(sleepTime);
            }

            //Should_Query_Typed_Data();
        }

        private static ComputerInfo[] CreateTypedRowsStartingAt(DateTime start, int rows)
        {
            var rng = new System.Random();
            var regions = new[] { "west-eu", "north-eu", "west-us", "east-us", "asia" };
            var hosts = new[] { "some-host", "some-other-host" };

            var timestamp = start;
            var infos = new ComputerInfo[rows];
            for (int i = 0; i < rows; i++)
            {
                int ram = rng.Next(1, 12);
                int cpu = rng.Next(1, 5);
                string region = regions[rng.Next(regions.Length)];
                string host = hosts[rng.Next(hosts.Length)];

                var info = new ComputerInfo { Timestamp = timestamp, CPU = cpu, RAM = ram, Host = host, Region = region };
                infos[i] = info;

                timestamp = timestamp.AddSeconds(1);
            }

            return infos;
        }

        private static ComputerInfo[] CreateTypedRowStartingAt(DateTime start)
        {
            var rng = new System.Random();
            var regions = new[] { "west-eu", "north-eu", "west-us", "east-us", "asia" };
            var hosts = new[] { "some-host", "some-other-host" };

            var timestamp = start;
            var infos = new ComputerInfo[1];

            int ram = rng.Next(1, 10);
            int cpu = rng.Next(1, 5);
            string region = regions[rng.Next(regions.Length)];
            string host = hosts[rng.Next(hosts.Length)];

            var info = new ComputerInfo { Timestamp = timestamp, CPU = cpu, RAM = ram, Host = host, Region = region };
            infos[0] = info;

            return infos;
        }



        public async static Task Should_Query_Typed_Data()
        {
            var client = new InfluxClient(new Uri("http://localhost:8086"));

            var resultSet = await client.ReadAsync<ComputerInfo>("mydb", "SELECT * FROM comp");

            // resultSet will contain 1 result in the Results collection (or multiple if you execute multiple queries at once)
            var result = resultSet.Results[0];

            // result will contain 1 series in the Series collection (or potentially multiple if you specify a GROUP BY clause)
            var series = result.Series[0];

            // series.Rows will be the list of ComputerInfo that you queried for
            foreach (var row in series.Rows)
            {
                Debug.Log("Timestamp: " + row.Timestamp);
                Debug.Log("CPU: " + row.CPU);
                // ...
            }
        }
    }


    public class ComputerInfo
    {
        [InfluxTimestamp]
        public DateTime Timestamp { get; set; }

        [InfluxTag("host")]
        public string Host { get; set; }

        [InfluxTag("region")]
        public string Region { get; set; }

        [InfluxField("cpu")]
        public int CPU { get; set; }

        [InfluxField("ram")]
        public int RAM { get; set; }
    }
}
