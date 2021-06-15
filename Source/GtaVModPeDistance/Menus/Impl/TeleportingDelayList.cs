using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class TeleportingDelayListItem : MenuListItem, MenuSettingItem
    {
        public TeleportingDelayListItem(): base("Teleporting Delay: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfTeleportingDelay = new List<dynamic>();
            for (int i = 0; i <= 100; i++)
            {
                listOfTeleportingDelay.Add(i);
            }
            return listOfTeleportingDelay;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.TeleportingDelay = (int) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.TeleportingDelay);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
