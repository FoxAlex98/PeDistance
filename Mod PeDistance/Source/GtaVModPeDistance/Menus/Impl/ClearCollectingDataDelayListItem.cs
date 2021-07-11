using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class ClearCollectingDataDelayListItem : MenuListItem, MenuSettingItem
    {
        public ClearCollectingDataDelayListItem(): base("Clear Collecting Data Delay: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfClearCollectingDataDelay = new List<dynamic>();
            for (int i = 0; i <= 100; i++)
            {
                listOfClearCollectingDataDelay.Add(i);
            }
            return listOfClearCollectingDataDelay;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.ClearCollectingDataDelay = (int) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.ClearCollectingDataDelay);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
