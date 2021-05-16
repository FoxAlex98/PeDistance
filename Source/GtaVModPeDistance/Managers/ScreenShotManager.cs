using GTA;
using GTA.Math;
using GtaVModPeDistance.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

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

        private static ScreenShotManager _instance;
        public static ScreenShotManager GetInstance()
        {
            if (_instance == null)
                _instance = new ScreenShotManager();
            return _instance;
        }

        private ScreenShotManager()
        {
            mainFolder = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", Settings.DirectoryName, "images");
            if (!Directory.Exists(mainFolder)) Directory.CreateDirectory(mainFolder);
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;

        }
    
        public ScreenShot TakeScreenshot(bool notification = false)
        {
            Vector3 actualPlayerPosition = Game.Player.Character.Position;
            memoryImage = new Bitmap(screenWidth, screenHeight);
            size = new Size(memoryImage.Width, memoryImage.Height);
            memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);
            string finalName = FileNameFormatter(World.GetStreetName(actualPlayerPosition), World.GetZoneLocalizedName(actualPlayerPosition));
            if (Settings.SaveScreenShotLocally)
            {
                fileName = Path.Combine(mainFolder, finalName);
                memoryImage.Save(string.Format(fileName));
            }                                             
            if(notification) GTA.UI.Notification.Show("ScreenShot Saved");
            return new ScreenShot(finalName, Utilities.ToBase64String(memoryImage, Settings.ImageFormat.Equals("Png")? ImageFormat.Png : ImageFormat.Jpeg));
        }

        public ScreenShot TakeScreenshot(Vector2 BottomLeft, Vector2 TopRight, bool notification = false)
        {
            Vector3 actualPlayerPosition = Game.Player.Character.Position;
            memoryImage = new Bitmap(screenWidth, screenHeight);
            size = new Size(memoryImage.Width, memoryImage.Height);
            memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);
            string finalName = FileNameFormatter(World.GetStreetName(actualPlayerPosition), World.GetZoneLocalizedName(actualPlayerPosition));

            memoryImage.SetPixel(0, 0, Color.Red);

            // TODO da pulire
            int maxX, minX, maxY, minY;
            if(BottomLeft.X > TopRight.X)
            {
                maxX = (int) BottomLeft.X;
                minX = (int) TopRight.X;
            } else
            {
                maxX = (int)TopRight.X;
                minX = (int)BottomLeft.X;
            }
            if (BottomLeft.Y > TopRight.Y)
            {
                maxY = (int)BottomLeft.Y;
                minY = (int)TopRight.Y;
            }
            else
            {
                maxY = (int)TopRight.Y;
                minY = (int)BottomLeft.Y;
            }

            for (int x = minX; x < maxX; x++)
            {
                memoryImage.SetPixel(x, maxY, Color.Red);
                memoryImage.SetPixel(x, minY, Color.Red);
            }

            for (int y = minY; y < maxY; y++)
            {
                memoryImage.SetPixel(minX, y, Color.Red);
                memoryImage.SetPixel(maxX, y, Color.Red);
            }

            if (Settings.SaveScreenShotLocally)
            {
                fileName = Path.Combine(mainFolder, finalName);
                memoryImage.Save(string.Format(fileName));
            }

            GTA.UI.Notification.Show("TopRight: " + TopRight.ToString());
            GTA.UI.Notification.Show("BottomLeft: " + BottomLeft.ToString());

            if (notification) GTA.UI.Notification.Show("ScreenShot Saved");
            return new ScreenShot(finalName, Utilities.ToBase64String(memoryImage, Settings.ImageFormat.Equals("Png") ? ImageFormat.Png : ImageFormat.Jpeg));
        }

        public string FileNameFormatter(string streetName, string zoneName)
        {
            streetName = streetName.Replace(" ", "_");
            zoneName = zoneName.Replace(" ", "_");
            return (index++).ToString() + "_" + streetName + "_" + zoneName + "." + (Settings.ImageFormat.Equals("Png") ? "png" : "jpg");
        }      
        
        public void DeleteAllScreenShot()
        {
            Array.ForEach(Directory.GetFiles(mainFolder),
              delegate (string path) {
                  FileInfo file = new FileInfo(path);
                  file.Delete();
              });
            index = 0;
        }

    }


}
