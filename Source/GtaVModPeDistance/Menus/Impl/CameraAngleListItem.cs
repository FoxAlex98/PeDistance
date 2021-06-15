using GTA;
using GTA.Math;
using NativeUI;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class CameraAngleListItem : MenuListItem, MenuSettingItem
    {
        public CameraAngleListItem(): base("Camera Angle: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfCameraAngle = new List<dynamic>();
            for (int i = -90; i <= 90; i++)
            {
                listOfCameraAngle.Add(i);
            }
            return listOfCameraAngle;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex)
        {
            Vector3 cameraRot = World.RenderingCamera.Rotation;
            World.RenderingCamera.Rotation = new Vector3((int) sender.Items[newIndex], cameraRot.Y, cameraRot.Z);
        }

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.CameraAngle = (int) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.CameraAngle);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
