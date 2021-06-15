using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class RenderingDelayListItem : MenuListItem, MenuSettingItem
    {
        public RenderingDelayListItem(): base("Rendering Delay: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfRenderingDelay = new List<dynamic>();
            for (int i = 0; i <= 100; i++)
            {
                listOfRenderingDelay.Add(i);
            }
            return listOfRenderingDelay;
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.RenderingDelay = (int) GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.RenderingDelay);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
