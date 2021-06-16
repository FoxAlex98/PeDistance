using GTA;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance.Menus.Impl
{
    class VehicleListItem : MenuListItem
    {
        public Globals.VehicleType VehicleType { get; set; }
        public VehicleListItem(string name, Globals.VehicleType vehicleType)
        {
            VehicleType = vehicleType;
            InitMenuListItem(name, 1);
        }

        protected override List<dynamic> InitList()
        {
            VehicleHash[] allVehiclesHash = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
            List<dynamic> listOfVehicle = new List<dynamic>();

            for (int i = 0; i < allVehiclesHash.Length; i++)
            {
                if (allVehiclesHash[i].IsVehicleType(VehicleType))
                    listOfVehicle.Add(allVehiclesHash[i]);
            }
            return listOfVehicle;
        }

        public override void OnClick()
        {
            int hash = (Model) GetCurrentListItem();
            VehicleHash vehicleHash = (VehicleHash) hash;
            UtilsFunctions.SpawnVehicle(vehicleHash);
        }

        public override void OnListChanged(UIMenuListItem sender, int newIndex){}
        
    }

}
