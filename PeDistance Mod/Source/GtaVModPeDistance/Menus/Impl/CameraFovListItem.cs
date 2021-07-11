using GTA;
using GtaVModPeDistance.CollectingSteps;
using NativeUI;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class CameraFovListItem : MenuListItem, MenuSettingItem
    {
        public CameraFovListItem(): base("Camera Fov: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfCameraFov = new List<dynamic>();
            for (int i = 20; i <= 90; i += 5)
            {
                listOfCameraFov.Add(i);
            }
            return listOfCameraFov;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex)
        {
            World.RenderingCamera.FieldOfView = (int) sender.Items[newIndex];
            //UtilsFunctions.SpawnSettingPeds(); //TODO: fix
        }

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            if(Settings.CameraFov != (int)GetList()[index])
            {
                Settings.CameraFov = (int) GetList()[index];
                CollectingState.PeDistance = Utilities.GetYMinByFov();
            }
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.CameraFov);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
