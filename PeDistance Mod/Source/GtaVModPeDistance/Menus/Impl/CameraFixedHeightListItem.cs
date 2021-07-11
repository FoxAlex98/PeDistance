using GTA;
using GTA.Math;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class CameraFixedHeightListItem : MenuListItem, MenuSettingItem
    {
        public CameraFixedHeightListItem(): base("Camera Fixed Height: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfCameraFixedHeight = new List<dynamic>();
            for (float i = 0; i <= 15; i += 0.1f)
            {
                listOfCameraFixedHeight.Add((float)Math.Round(i, 1));
            }
            return listOfCameraFixedHeight;
        }

        public override void OnClick(){}
        public override void OnListChanged(UIMenuListItem sender, int newIndex)
        {
            float Z = World.GetGroundHeight(Game.Player.Character.Position) + (float) sender.Items[newIndex];
            Vector3 cameraPos = World.RenderingCamera.Position;
            World.RenderingCamera.Position = new Vector3(cameraPos.X, cameraPos.Y, Z);
        }

        public void Save()
        {
            int index = ((UIMenuListItem)Item).Index;
            Settings.CameraFixedHeight = (float) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem)Item).Items.IndexOf(Settings.CameraFixedHeight);
            ((UIMenuListItem)Item).Index = index;
        }

    }
}
