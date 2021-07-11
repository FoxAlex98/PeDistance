using NativeUI;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class MaxCollectedDataListItem : MenuListItem, MenuSettingItem
    {
        public MaxCollectedDataListItem(): base("Max Collected Data: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfMaxCollectedData = new List<dynamic>{ 5 , 10 };
            for (int i = 30; i <= Settings.MaxCollectedSelectionable; i += 30)
            {
                listOfMaxCollectedData.Add(i);
            }
            return listOfMaxCollectedData;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem)Item).Index;
            Settings.MaxCollectedData = (int)GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem)Item).Items.IndexOf(Settings.MaxCollectedData);
            ((UIMenuListItem)Item).Index = index;
        }
    }
}