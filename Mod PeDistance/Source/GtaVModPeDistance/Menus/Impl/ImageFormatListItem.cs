using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class ImageFormatListItem : MenuListItem, MenuSettingItem
    {
        public ImageFormatListItem(): base("Image Format: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            return new List<dynamic> { "Jpeg", "Png" };
        }

        public override void OnClick(){}

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem) Item).Index;
            Settings.ImageFormat = GetList()[index];
        }

        public void Load()
        {
            int index = ((UIMenuListItem) Item).Items.IndexOf(Settings.ImageFormat);
            ((UIMenuListItem) Item).Index = index;
        }
    }
}
