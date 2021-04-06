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
        private CsvWriter csvWriter;
        private string filePath;
        public DataMananger()
        {
            data = new List<Data>();
            filePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", "Dataset.csv");
            CleanFile();
            setInstance();                      
        }

        public void setInstance()
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

        public void addElement(Data element)
        {
            data.Add(element);
        }        

        public void WriteDataToFile()
        {
            if (data.Count != 0)
            {
                csvWriter.WriteRecords(data);
                csvWriter.Flush();
                data = new List<Data>();
            }
          
        }

        public void CleanFile(bool createNew = false)
        {
            FileInfo file = new FileInfo(filePath);
            file.Delete();
            if(createNew) setInstance();            
        }

        public void FreeResource()
        {
            csvWriter.Dispose();
        }
    }
}
