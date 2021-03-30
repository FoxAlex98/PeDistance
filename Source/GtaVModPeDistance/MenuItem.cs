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
    public class MenuItem
    {
        private UIMenuItem item;
        private string name;
        private ActionToDo action;
        
        public MenuItem(string name, ActionToDo action) //optional description?
        {
            this.name = name;
            this.action = action;

            item = new UIMenuItem(name);
        }

        public void Evaluate(UIMenuItem itemToCompare)
        {
            if(item == itemToCompare)
            {
                action();
            }
        }

        public UIMenuItem GetItem()
        {
            return item;
        }
    }
}
