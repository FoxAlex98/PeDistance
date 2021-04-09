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
        public List<SpawnPoint> saveSpawnPoints;
        CsvConfiguration config;

        public LocationManager() {

            string mainFolder = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", Settings.DirectoryName, "data");
            if (!Directory.Exists(mainFolder)) Directory.CreateDirectory(mainFolder);
            filePath = Path.Combine(mainFolder, "Locations.csv");
            //CleanFile();
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            InitReader();
            allSpawnPoints = GetAllSpawnPoint();
            saveSpawnPoints = new List<SpawnPoint>(allSpawnPoints);
            InitWriter();
            rand = new Random();
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

        public void SaveCoordinates(SpawnPoint spawnPlace)
        {
            saveSpawnPoints.Add(spawnPlace);
            allSpawnPoints.Add(spawnPlace);
            Notification.Show("Player coord saved");
            SaveRecords();
        }

        private void SaveRecords()
        {
            csvWriter.WriteRecords(saveSpawnPoints);
            csvWriter.Flush();
            saveSpawnPoints.Clear();
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
