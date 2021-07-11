using NativeUI;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class CollectingDataDelayListItem : MenuListItem, MenuSettingItem
    {
        public CollectingDataDelayListItem(): base("Collecting Data Delay: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfCollectingDataDelay = new List<dynamic>();
            for (int i = 0; i <= 100; i++)
            {
                listOfCollectingDataDelay.Add(i);
            }
            return listOfCollectingDataDelay;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.CollectingDataDelay = (int) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.CollectingDataDelay);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
