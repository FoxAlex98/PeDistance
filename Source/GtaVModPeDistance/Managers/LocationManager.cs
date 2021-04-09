using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GTA.Math;
using GTA.UI;
using GtaVModPeDistance.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace GtaVModPeDistance
{
    class LocationManager
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        Random rand;
        int index = 2;
        public List<SpawnPoint> allSpawnPoints;

        public LocationManager() {
            SaveLocationFile();
            allSpawnPoints = GetAllSpawnPoint();
            rand = new Random();
        }

        public void SaveLocationFile()
        {
            xlApp = new Excel.Application();
            if (xlApp == null)
            {
                Console.WriteLine("Excel is not properly installed!!");
                Screen.ShowSubtitle("Excel problems");
                return;
            }
            string assemblyFolder = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string xmlFileName = System.IO.Path.Combine(assemblyFolder, "scripts", "Locations.csv");
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            try
            {
                xlWorkBook = xlApp.Workbooks.Open(xmlFileName);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Notification.Show("File Locations.csv opened! You can start saving positions. Close File when you are done.");
            }
            catch (Exception e)
            {
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "Pos X";
                xlWorkSheet.Cells[1, 2] = "Pos Y";
                xlWorkSheet.Cells[1, 3] = "Pos Z";
                xlWorkSheet.Cells[1, 4] = "Rot X";
                xlWorkSheet.Cells[1, 5] = "Rot Y";
                xlWorkSheet.Cells[1, 6] = "Rot Z";
                xlWorkSheet.Cells[1, 7] = "Street Name";
                xlWorkSheet.Cells[1, 8] = "Localized Name";
                xlWorkSheet.Cells[1, 9] = index.ToString();
                xlWorkBook.SaveAs(xmlFileName, Excel.XlFileFormat.xlCSV);
                Notification.Show("File Locations.csv created! You can start saving positions. Close File when you are done.");
            }
            finally
            {
                index = (int)((xlWorkSheet.Cells[1, 9] as Excel.Range).Value);
                Notification.Show(index - 2 + " location already saved");
            }

        }

        public void CloseLocationFile()
        {
            if (xlApp != null)
            {
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
             
                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
                xlApp = null;
                Notification.Show("File closed.");
            }
            else
            {
                Notification.Show("File already closed.");
            }
        }

        public void SaveCoordinates(SpawnPoint spawnPlace)
        {
            xlWorkSheet.Cells[index, 1] = spawnPlace.Position.X;
            xlWorkSheet.Cells[index, 2] = spawnPlace.Position.Y;
            xlWorkSheet.Cells[index, 3] = spawnPlace.Position.Z;
            xlWorkSheet.Cells[index, 4] = spawnPlace.Rotation.X;
            xlWorkSheet.Cells[index, 5] = spawnPlace.Rotation.Y;
            xlWorkSheet.Cells[index, 6] = spawnPlace.Rotation.Z;
            xlWorkSheet.Cells[index, 7] = spawnPlace.StreetName;
            xlWorkSheet.Cells[index, 8] = spawnPlace.ZoneLocalizedName;
            xlWorkSheet.Cells[1, 9] = (++index).ToString();
            xlWorkBook.Save();
            Notification.Show("Player coord saved ~o~" + spawnPlace.Position.ToString());
        }

        public void DeleteLastCoordinate()
        {
            xlWorkSheet.Cells[1, 9] = (--index).ToString();
            xlWorkSheet.Cells[index, 1] = "";
            xlWorkSheet.Cells[index, 2] = "";
            xlWorkSheet.Cells[index, 3] = "";
            xlWorkSheet.Cells[index, 4] = "";
            xlWorkSheet.Cells[index, 5] = "";
            xlWorkSheet.Cells[index, 6] = "";
            xlWorkSheet.Cells[index, 7] = "";
            xlWorkSheet.Cells[index, 8] = "";           
            xlWorkBook.Save();
            Notification.Show("Last coord deleted");
        }

        private List<SpawnPoint> GetAllSpawnPoint()
        {
            List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

            for(int i = 2; i < index; i++)
            {
                float posX = (float) ((xlWorkSheet.Cells[i, 1] as Excel.Range).Value);
                float posY = (float) ((xlWorkSheet.Cells[i, 2] as Excel.Range).Value);
                float posZ = (float) ((xlWorkSheet.Cells[i, 3] as Excel.Range).Value);
                float rotX = (float) ((xlWorkSheet.Cells[i, 4] as Excel.Range).Value);
                float rotY = (float) ((xlWorkSheet.Cells[i, 5] as Excel.Range).Value);
                float rotZ = (float) ((xlWorkSheet.Cells[i, 6] as Excel.Range).Value);
                string streetName = (string)((xlWorkSheet.Cells[i, 7] as Excel.Range).Value);
                string zoneLocalizedName = (string)((xlWorkSheet.Cells[i, 8] as Excel.Range).Value);
                Vector3 pos = new Vector3(posX, posY, posZ);            
                Vector3 rot = new Vector3(rotX, rotY, rotZ);
                spawnPoints.Add(new SpawnPoint(pos, rot, streetName, zoneLocalizedName));
            }

            return spawnPoints;
        }

        public SpawnPoint GetRandomPoint()
        {
            int randomIndex = rand.Next(0, allSpawnPoints.Count);
            return allSpawnPoints[randomIndex];
        }
    }
}
