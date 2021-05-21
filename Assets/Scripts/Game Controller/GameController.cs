using Assets.Scripts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vibrant.InfluxDB.Client;

namespace Assets.Scripts.Game_Controller
{
    public class GameController
    {
        #region Singleton
        private static GameController instance;
        public static GameController Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameController();
                return instance;
            }
        }

        private GameController() 
        {
            Initialize();
        }
        #endregion

        public InfluxClient Client { get; private set; }
        public const string DATABASE_NAME = "airport";
        public const string AIRPLANE_MEASUREMENT = "airplanes";

        public const string STREAM_AIRPLANE_POSITIONS = "stream_airplanes_positions";
        public const string STORAGE_AIRPLANE_BOUNDING_BOX = "storage_airplanes_in_bounding_box";

        public const string STORAGE_HEMISPHERES_COUNT = "storage_hemispheres_count";
        public const string STORAGE_HEMISPHERES = "storage_hemispheres";

        public const string STREAM_AIRPLANE_PROGRESS = "stream_airplanes_progress";
        public const string STORAGE_CLOSE_TO_DESTINATION = "storage_close_to_destination";
        public const string STORAGE_STDDEV_DESTINATIONS = "storage_stddev_destinations";

        private void Initialize()
        {
            Client = new InfluxClient(new Uri("http://localhost:8086"));
        }        

        public async Task WriteAirplaneAsync(Airplane[] airplanes)
        {
            await Client.WriteAsync(DATABASE_NAME, AIRPLANE_MEASUREMENT, airplanes);
        }

        public async Task WriteAirplanePositionAsync(AirplanePosition[] airplanePositions)
        {
            await Client.WriteAsync(DATABASE_NAME, STREAM_AIRPLANE_POSITIONS, airplanePositions);
        }

        public async Task WriteAirplaneProgressAsync(AirplaneProgress[] airplanesProgress)
        {
            await Client.WriteAsync(DATABASE_NAME, STREAM_AIRPLANE_PROGRESS, airplanesProgress);
        }

        public void DropAllMeasurements()
        {
            _ = DropAirplanesAsync();
            _ = DropAirplanesPositionsAsync();
            _ = DropHemispheresAsync();
            _ = DropProgressAsync();
        }

        private async Task DropAirplanesAsync()
        {
            await Client.DropMeasurementAsync(DATABASE_NAME, AIRPLANE_MEASUREMENT);
        }

        private async Task DropAirplanesPositionsAsync()
        {
            await Client.DropMeasurementAsync(DATABASE_NAME, STORAGE_AIRPLANE_BOUNDING_BOX);
            await Client.DropMeasurementAsync(DATABASE_NAME, STREAM_AIRPLANE_POSITIONS);
        }

        private async Task DropHemispheresAsync()
        {
            await Client.DropMeasurementAsync(DATABASE_NAME, STORAGE_HEMISPHERES_COUNT);
            await Client.DropMeasurementAsync(DATABASE_NAME, STORAGE_HEMISPHERES);
        }

        private async Task DropProgressAsync()
        {
            await Client.DropMeasurementAsync(DATABASE_NAME, STREAM_AIRPLANE_PROGRESS);
            await Client.DropMeasurementAsync(DATABASE_NAME, STORAGE_CLOSE_TO_DESTINATION);
            await Client.DropMeasurementAsync(DATABASE_NAME, STORAGE_STDDEV_DESTINATIONS);
        }

    }
}
