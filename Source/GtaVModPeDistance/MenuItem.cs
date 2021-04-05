using NativeUI;

namespace GtaVModPeDistance
{
    public class MenuItem
    {
        private UIMenuItem _item;
        public UIMenuItem Item
        {
            get => _item;
            set => _item = value;
        }
        private string name;
        private ActionToDo action;
        
        public MenuItem(string name, ActionToDo action) //optional description?
        {
            this.name = name;
            this.action = action;

            _item = new UIMenuItem(name);
        }

        public MenuItem(UIMenuItem item, ActionToDo action = null)
        {
            _item = item;
            this.action = action;
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
