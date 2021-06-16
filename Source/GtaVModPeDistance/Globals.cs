using GTA;
using GTA.Native;

namespace GtaVModPeDistance
{
    class Globals
    {
        public static Vehicle Vehicle { get; set; }

        public static void ShowHud()
        {
            Function.Call(Hash.DISPLAY_RADAR, true);
        }

        public static void HideHud()
        {
            Function.Call(Hash.DISPLAY_RADAR, false);
        }

        public enum VehicleType
        {
            CARS,
            BOATS,
            MOTORBIKES,
            HELICOPTERS,
            PLANES,
        }

    }
}
