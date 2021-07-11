using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class WeatherListItem : MenuListItem
    {
        public WeatherListItem(): base("Weather: ")
        {

        }

        protected override List<dynamic> InitList()
        {
            Weather[] allWeather = (Weather[])Enum.GetValues(typeof(Weather));
            List<dynamic> listOfWeather = new List<dynamic>();

            for (int i = 0; i < allWeather.Length; i++)
                listOfWeather.Add(allWeather[i]);

            return listOfWeather;
        }

        public override void OnClick()
        {
            World.TransitionToWeather(GetCurrentListItem(), 0f);
        }

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}
    }
}
