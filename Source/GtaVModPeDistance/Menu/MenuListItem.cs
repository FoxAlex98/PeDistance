using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.Menu
{
    public abstract class MenuListItem : MenuItem
    {
        private List<dynamic> list;

        public MenuListItem(string name, int index = 1)
        {
            Item = new UIMenuListItem(name, GetList(), index);
            action = () => OnClick();
        }

        public List<dynamic> GetList()
        {
            if (list == null)
                list = InitList();
            return list;
        }

        protected dynamic GetCurrentListItem()
        {
            return list[((UIMenuListItem) Item).Index];
        }

        protected abstract List<dynamic> InitList();

        public abstract void OnListChanged();

        public abstract void OnClick();
    }
}
