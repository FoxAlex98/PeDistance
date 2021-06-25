using CsvHelper;
using CsvHelper.Configuration;
using GtaVModPeDistance.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GtaVModPeDistance.File
{
    class DataMananger
    {

        private List<Data> data;
        private CsvWriter csvWriter = null;
        private CsvConfiguration config;
        private string filePath;
        private FileInfo file;

        private static DataMananger _instance;
        public static DataMananger GetInstance()
        {
            if (_instance == null)
                _instance = new DataMananger();
            return _instance;
        }
        private DataMananger()
        {
            data = new List<Data>();
            string mainFolder = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", Settings.DirectoryName, "data");
            if (!Directory.Exists(mainFolder)) Directory.CreateDirectory(mainFolder);
            filePath = Path.Combine(mainFolder, "Dataset.csv");
            file = new FileInfo(filePath);
            SetFileConfig(!file.Exists);
            InitWriter();
        }

        private void SetFileConfig(bool hasRecord)
        {
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasRecord,
            };
        }

        public void InitWriter()
        {
            try
            {
                csvWriter = new CsvWriter(new StreamWriter(filePath, true), config);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("FileNotFoundException: " + e.ToString());
            }
        }

        public void AddElement(Data element)
        {
            data.Add(element);
            if (data.Count % 5 == 0)
                WriteDataToFile();
                   
        }        

        public void WriteDataToFile()
        {
            if (data.Count != 0)
            {
                csvWriter.WriteRecords(data);
                csvWriter.Flush();
                data.Clear();
            }
          
        }

        public void CleanFile()
        {
            if(csvWriter != null)
            {
                FreeResource();
                if (file.Exists) file.Delete();                
            }
            SetFileConfig(true);
            InitWriter();
        }

        public void FreeResource()
        {
            csvWriter.Dispose();
        }
    }
}
