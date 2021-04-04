using System;
using System.Runtime.InteropServices;
using GTA.Math;
using GTA.UI;
using Excel = Microsoft.Office.Interop.Excel;

namespace GtaVModPeDistance
{
    class FileManager
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        int index = 2;

        public FileManager() {
            SaveLocationFile();
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
            string xmlFileName = System.IO.Path.Combine(assemblyFolder, "scripts", "Location.xls");
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            try
            {
                xlWorkBook = xlApp.Workbooks.Open(xmlFileName);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Notification.Show("File Location.xls opened! You can start saving positions. Close File when you are done.");
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
                xlWorkBook.SaveAs(xmlFileName);
                Notification.Show("File Location.xls created! You can start saving positions. Close File when you are done.");
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

                xlApp = null;

                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
                Notification.Show("File closed.");
            }
            else
            {
                Notification.Show("File already closed.");
            }
        }

        public void SaveCoordinates(Vector3 pos, Vector3 rot, string streetName, string zoneLocalizedName)
        {
            xlWorkSheet.Cells[index, 1] = pos.X;
            xlWorkSheet.Cells[index, 2] = pos.Y;
            xlWorkSheet.Cells[index, 3] = pos.Z;
            xlWorkSheet.Cells[index, 4] = pos.X;
            xlWorkSheet.Cells[index, 5] = pos.Y;
            xlWorkSheet.Cells[index, 6] = pos.Z;
            xlWorkSheet.Cells[index, 7] = streetName;
            xlWorkSheet.Cells[index, 8] = zoneLocalizedName;
            xlWorkSheet.Cells[1, 9] = (++index).ToString();
            xlWorkBook.Save();
            Notification.Show("Player coord saved ~o~" + pos.ToString());
        }
    }
}
