using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance.File
{
    class ScreenShotManager
    {
        private string mainFolder, fileName;
        private int screenWidth, screenHeight;
        private Bitmap memoryImage;
        private Size size;
        private Graphics memoryGraphics;

        int index = 0;


        public ScreenShotManager()
        {
            mainFolder = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", "images");
            
            screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

        }
    
        public void TakeScreenshot(SpawnPoint spawnPoint)
        {
            memoryImage = new Bitmap(screenWidth, screenHeight);
            size = new Size(memoryImage.Width, memoryImage.Height);
            memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);

            fileName = Path.Combine(mainFolder, FileNameFormatter(spawnPoint.StreetName, spawnPoint.ZoneLocalizedName));

            memoryImage.Save(string.Format(fileName));
            GTA.UI.Notification.Show("ScreenShot Saved");
        }

        public string FileNameFormatter(string streetName, string zoneName)
        {
            streetName = streetName.Replace(" ", "_");
            zoneName = zoneName.Replace(" ", "_");
            return streetName + "_" + zoneName + "_" + (index++).ToString() + ".png";
        }
    
    }


}
