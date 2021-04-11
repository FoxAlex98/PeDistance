using CsvHelper;
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
        private string filePath;

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
            filePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", "Dataset.csv");
            string mainFolder = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", Settings.DirectoryName, "data");
            if (!Directory.Exists(mainFolder)) Directory.CreateDirectory(mainFolder);
            filePath = Path.Combine(mainFolder, "Dataset.csv");
            CleanFile();
        }

        public void InitWriter()
        {
            try
            {
                csvWriter = new CsvWriter(new StreamWriter(filePath), CultureInfo.InvariantCulture);
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
                FileInfo file = new FileInfo(filePath);
                if (file.Exists) file.Delete();                
            }
            InitWriter();
        }

        public void FreeResource()
        {
            csvWriter.Dispose();
        }
    }
}
