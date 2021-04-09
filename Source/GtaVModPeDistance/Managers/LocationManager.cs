using System;
using System.Linq;
using System.Collections.Generic;
using GTA.UI;
using GtaVModPeDistance.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

namespace GtaVModPeDistance
{
    class LocationManager
    {
        private CsvWriter csvWriter = null;
        private CsvReader csvReader = null;
        private string filePath;
        Random rand;
        public List<SpawnPoint> allSpawnPoints;
        CsvConfiguration config;

        public LocationManager() {
            filePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", "Locations.csv");
            //CleanFile();
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            InitReader();
            allSpawnPoints = GetAllSpawnPoint();
            InitWriter();
            rand = new Random();
        }

        private void InitWriter()
        {
            if (csvWriter != null) return;

            try
            {
                if (csvReader != null)
                {
                    csvReader.Dispose();
                    csvReader = null;
                }
                csvWriter = new CsvWriter(new StreamWriter(filePath), config);
                SaveRecords();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + e.ToString());
            }
        }
        
        private void InitReader()
        {
            if (csvReader != null) return;

            try
            {
                if(csvWriter != null)
                {
                    csvWriter.Dispose();
                    csvWriter = null;
                }
                csvReader = new CsvReader(new StreamReader(filePath), config);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + e.ToString());
            }
        }


        public void SaveCoordinates(SpawnPoint spawnPlace)
        {
            allSpawnPoints.Add(spawnPlace);
            Notification.Show("Player coord saved ~o~");
            SaveRecords();
        }

        private void SaveRecords()
        {
            csvWriter.WriteRecords(allSpawnPoints);
            csvWriter.Flush();
            allSpawnPoints.Clear();
        }

        private List<SpawnPoint> GetAllSpawnPoint()
        {
            if (csvReader == null) return new List<SpawnPoint>();
            return csvReader.GetRecords<SpawnPoint>().ToList();
        }

        public SpawnPoint GetRandomPoint()
        {
            int randomIndex = rand.Next(0, allSpawnPoints.Count);
            return allSpawnPoints[randomIndex];
        }

        public void CleanFile(bool createNew = false)
        {
            FileInfo file = new FileInfo(filePath);
            file.Delete();
            if (createNew) InitWriter();
        }
    }
}
