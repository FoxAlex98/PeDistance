using System;
using System.Runtime.InteropServices;
using GTA.Math;
using GTA.UI;
using GtaVModPeDistance.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace GtaVModPeDistance
{
    class FileManager
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        Random rand;
        int index = 2;

        public FileManager() {
            SaveLocationFile();
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

        public SpawnPoint getRandomPoint()
        {
            int randomIndex = rand.Next(2, index - 1);
            return new SpawnPoint(
                new Vector3(
                    (float) ((xlWorkSheet.Cells[randomIndex, 1] as Excel.Range).Value), 
                    (float) ((xlWorkSheet.Cells[randomIndex, 2] as Excel.Range).Value),
                    (float) ((xlWorkSheet.Cells[randomIndex, 3] as Excel.Range).Value)),
                new Vector3(
                    (float) ((xlWorkSheet.Cells[randomIndex, 4] as Excel.Range).Value),
                    (float) ((xlWorkSheet.Cells[randomIndex, 5] as Excel.Range).Value),
                    (float) ((xlWorkSheet.Cells[randomIndex, 6] as Excel.Range).Value)),
                ((xlWorkSheet.Cells[randomIndex, 7] as Excel.Range).Value),
                ((xlWorkSheet.Cells[randomIndex, 8] as Excel.Range).Value)
                );
        }
    }
}
