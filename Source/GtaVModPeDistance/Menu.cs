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
        private UIMenu menu;
        private List<MenuItem> itemList;

        public Menu(string menuName, string menuSubtitle, List<MenuItem> itemList)
        {
            menu = new UIMenu(menuName, menuSubtitle);
            menu.OnItemSelect += onMainMenuItemSelected;
            this.itemList = itemList;
            itemList.ForEach(x => menu.AddItem(x.GetItem()));
        }

        void onMainMenuItemSelected(UIMenu sender, UIMenuItem item, int index)
        {
            itemList.ForEach(x => x.Evaluate(item));
        }

        public UIMenu GetMainMenu()
        {
            return menu;
        }

        public void ShowHideMenu()
        {
            menu.Visible = !menu.Visible;
        }

    }

    
}