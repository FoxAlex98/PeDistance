using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using NativeUI;

namespace GtaVModPeDistance
{
    public class Menu
    {
        private List<MenuItem> itemList;
        private UIMenu _modMenu;
        public UIMenu ModMenu
        {
            get => _modMenu;
            set => _modMenu = value;
        }


        public Menu(string menuName, string menuSubtitle, List<MenuItem> itemList)
        {
            _modMenu = new UIMenu(menuName, menuSubtitle);
            SetupMenu(_modMenu, itemList);
        }

        public Menu(UIMenu modMenu, List<MenuItem> itemList)
        {
            _modMenu = modMenu;
            SetupMenu(_modMenu, itemList);
        }

        private void SetupMenu(UIMenu modMenu, List<MenuItem> itemList)
        {
            _modMenu.OnItemSelect += onMainMenuItemSelected;
            this.itemList = itemList;
            if (itemList.Count > 0)
                itemList.ForEach(x => modMenu.AddItem(x.GetItem()));
        }

        void onMainMenuItemSelected(UIMenu sender, UIMenuItem item, int index)
        {
            itemList.ForEach(x => x.Evaluate(item));
        }

        public void ShowHideMenu()
        {
            _modMenu.Visible = !_modMenu.Visible;
        }

    }

    
}