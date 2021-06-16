using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class DayTimeListItem : MenuListItem
    {
        public DayTimeListItem(): base("DayTime: ")
        {

        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> dayTimeList = new List<dynamic>
            {
                "Morning",
                "Midday",
                "Afternoon",
                "Evening",
                "Midnight",
                "Night"
            };
            return dayTimeList;
        }

        public override void OnClick()
        {
            UtilsFunctions.SetTime(getTimeByString((string)GetCurrentListItem()));
        }

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        private int getTimeByString(string dayTime)
        {
            switch (dayTime)
            {
                case "Morning": return 8;
                case "Midday": return 12;
                case "Afternoon": return 16;
                case "Evening": return 20;
                case "Midnight": return 0;
                case "Night": return 4;
            }
            return 0;
        }

    }
}
