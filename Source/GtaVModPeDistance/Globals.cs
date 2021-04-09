using GTA;
using GTA.Native;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance
{
    class Globals
    {
        public static Ped Ped { get; set; }
        public static SpawnPoint SpawnPoint { get; set; }

        public static void ShowHud()
        {
            Function.Call(Hash.DISPLAY_RADAR, true);
        }

        public static void HideHud()
        {
            Function.Call(Hash.DISPLAY_RADAR, false);
        }
    }
}
