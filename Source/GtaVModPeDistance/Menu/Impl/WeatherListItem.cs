using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.Menu.Impl
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

        public override void OnListChanged()
        {
            throw new NotImplementedException();
        }


    }
}
