using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class PedSpawningDelayListItem : MenuListItem, MenuSettingItem
    {
        public PedSpawningDelayListItem(): base("Ped Spawning Delay: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfPedSpawningDelay = new List<dynamic>();
            for (int i = 0; i <= 100; i++)
            {
                listOfPedSpawningDelay.Add(i);
            }
            return listOfPedSpawningDelay;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.PedSpawningDelay = (int) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.PedSpawningDelay);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
