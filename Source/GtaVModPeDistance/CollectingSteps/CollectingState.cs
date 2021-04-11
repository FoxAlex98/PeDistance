using GTA;
using GTA.Math;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance.CollectingSteps
{
    class CollectingState
    {
        public static int CollectedDataCounter { get; set; }
        public static Ped Ped { get; set; }
        public static SpawnPoint SpawnPoint { get; set; }
        public static bool StartCollectingData { get; set; } = false;
        public static bool WannaStop { get; set; } = false;
        public static Vector3 InitialPosition { get; set; }
        public static CollectingStep ActualStep { get; set; }

    }
}
