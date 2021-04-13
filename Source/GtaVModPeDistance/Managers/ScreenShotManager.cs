﻿using GTA;
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
            screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

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
