using CsvHelper;
using CsvHelper.Configuration;
using GTA.UI;
using GtaVModPeDistance.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GtaVModPeDistance.File
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
        int index = -1;

        private static LocationManager _instance;
        public static LocationManager GetInstance() {
            if (_instance == null)
                _instance = new LocationManager();
            return _instance;
        }

        private LocationManager() {

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

        public SpawnPoint GetNextPoint()
        {
            index++;
            Notification.Show((index % allSpawnPoints.Count).ToString());
            return allSpawnPoints[index % allSpawnPoints.Count];
        }
        public SpawnPoint GetPrevPoint()
        {
            index--;
            if (index < 0) index = allSpawnPoints.Count - 1;
            Notification.Show((index % allSpawnPoints.Count).ToString());
            return allSpawnPoints[index % allSpawnPoints.Count];
        }
        
        public void CleanFile(bool createNew = false)
        {
            FileInfo file = new FileInfo(filePath);
            file.Delete();
            if (createNew) InitWriter();
        }
    }
}
