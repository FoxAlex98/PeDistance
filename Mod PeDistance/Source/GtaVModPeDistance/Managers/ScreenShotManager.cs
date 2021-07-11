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

        public void TakeScreenshot()
        {
            memoryImage = new Bitmap(screenWidth, screenHeight);
            size = new Size(memoryImage.Width, memoryImage.Height);
            memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);
        }

        public ScreenShot SaveScreenShot(bool notification = false)
        {
            Vector3 actualPlayerPosition = Game.Player.Character.Position;
            string finalName = FileNameFormatter(World.GetStreetName(actualPlayerPosition), World.GetZoneLocalizedName(actualPlayerPosition));

            if (Settings.SaveScreenShotLocally)
            {
                fileName = Path.Combine(mainFolder, finalName);
                memoryImage.Save(string.Format(fileName));
            }

            if (notification) GTA.UI.Notification.Show("ScreenShot Saved");
            return new ScreenShot(finalName, Utilities.ToBase64String(memoryImage, Settings.ImageFormat.Equals("Png") ? ImageFormat.Png : ImageFormat.Jpeg));
        }

        //TODO: parametrizzare la dimensione del border
        public void DrawBoundingBox(Vector2 bottomLeft, Vector2 topRight, Color color)
        {
            int xFix = topRight.X == Screen.PrimaryScreen.Bounds.Width ? -1 : 0;
            int yFix = bottomLeft.Y == Screen.PrimaryScreen.Bounds.Height ? -1 : 0;

            int border = 5; //meglio se dispari
            fillHorizontalLine(bottomLeft, topRight, color, yFix, border);
            fillVerticalLine(bottomLeft, topRight, color, xFix, border);
        }
        private void fillHorizontalLine(Vector2 bottomLeft, Vector2 topRight, Color color, int yFix, int border)
        {
            for (int x = (int) bottomLeft.X; x < topRight.X + border / 2; x++)
            {
                BorderFillerHorizontalLine(new Vector2(x, bottomLeft.Y + yFix), border, color);
                BorderFillerHorizontalLine(new Vector2(x, topRight.Y), border, color);
            }
        }

        private void fillVerticalLine(Vector2 bottomLeft, Vector2 topRight, Color color, int xFix, int border)
        {
            for (int y = (int) topRight.Y; y < bottomLeft.Y; y++)
            {
                BorderFillerVerticalLine(new Vector2(bottomLeft.X, y), border, color);
                BorderFillerVerticalLine(new Vector2(topRight.X + xFix, y), border, color);
            }
        }

        public void BorderFillerVerticalLine(Vector2 median, int border, Color color)
        {
            for(int i = (int) median.X - border/2; i < median.X + border/2; i++)
                if (i >= 0 && i < Screen.PrimaryScreen.Bounds.Width)
                    memoryImage.SetPixel(i, (int) median.Y, color);
        }
        
        public void BorderFillerHorizontalLine(Vector2 median, int border, Color color)
        {
            for (int i = (int) median.Y - border/2; i < median.Y + border/2; i++)
                if (i >= 0 && i < Screen.PrimaryScreen.Bounds.Height)
                    memoryImage.SetPixel((int) median.X, i, color);
        }

        public string FileNameFormatter(string streetName, string zoneName)
        {
            streetName = streetName.Replace(" ", "_");
            zoneName = zoneName.Replace(" ", "_");
            return DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "_fov-" + Settings.CameraFov +  "_" + streetName + "_" + zoneName + "." + (Settings.ImageFormat.Equals("Png") ? "png" : "jpg");
        }      
        
        public void DeleteAllScreenShot()
        {
            Array.ForEach(Directory.GetFiles(mainFolder),
              delegate (string path) {
                  FileInfo file = new FileInfo(path);
                  file.Delete();
              });
        }

    }


}
