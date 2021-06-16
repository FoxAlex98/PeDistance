using NativeUI;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus
{
    public abstract class MenuListItem : MenuItem
    {
        private List<dynamic> list;

        public MenuListItem(string name, int index = 1)
        {
            InitMenuListItem(name, index);
        }

        protected void InitMenuListItem(string name, int index)
        {
            Item = new UIMenuListItem(name, GetList(), index);
            ((UIMenuListItem)Item).OnListChanged += OnListChanged;
            action = () => OnClick();
        }

        public MenuListItem()
        {

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

        public abstract void OnListChanged(UIMenuListItem sender, int newIndex);

        public abstract void OnClick();
    }
}
