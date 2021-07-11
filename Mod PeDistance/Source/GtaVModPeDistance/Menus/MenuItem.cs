using NativeUI;

namespace GtaVModPeDistance.Menus
{
    public delegate void ActionToDo();
    public class MenuItem
    {
        private UIMenuItem _item;
        protected ActionToDo action;

        public MenuItem()
        {

        }

        public MenuItem(string name, ActionToDo action) //optional description?
        {
            this.action = action;
            _item = new UIMenuItem(name);
        }

        public MenuItem(UIMenuItem item, ActionToDo action = null)
        {
            _item = item;
            this.action = action;
        }
        public UIMenuItem Item
        {
            get => _item;
            set => _item = value;
        }

        public void Evaluate(UIMenuItem itemToCompare)
        {
            if(_item == itemToCompare)
            {
                if(action != null)
                {
                   action();
                }
            }
        }

    }
}
