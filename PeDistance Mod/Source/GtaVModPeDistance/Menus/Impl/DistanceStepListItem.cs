using GtaVModPeDistance.CollectingSteps;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class DistanceStepListItem : MenuListItem, MenuSettingItem
    {
        public DistanceStepListItem(): base("Distance Step: ")
        {
            GetList();
            Load();
        }

        protected override List<dynamic> InitList()
        {
            List<dynamic> listOfDistanceStep = new List<dynamic>();
            for (float i = 0; i <= 2f; i += 0.1f)
            {
                listOfDistanceStep.Add((float)Math.Round(i, 1));
            }
            return listOfDistanceStep;
        }

        public override void OnClick(){}
        public override void OnListChanged(UIMenuListItem sender, int newIndex){}

        public void Save()
        {
            int index = ((UIMenuListItem)Item).Index;
            if (Settings.DistanceStep == (float)GetList()[index])
            {
                Settings.DistanceStep = (float)GetList()[index];
                CollectingState.PeDistance = Utilities.GetYMinByFov();
            }
        }

        public void Load()
        {
            int index = ((UIMenuListItem)Item).Items.IndexOf(Settings.DistanceStep);
            ((UIMenuListItem)Item).Index = index;
        }

    }
}
