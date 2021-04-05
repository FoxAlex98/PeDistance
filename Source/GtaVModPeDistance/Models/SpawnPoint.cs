using GTA.Math;

namespace GtaVModPeDistance.Models
{
    class SpawnPoint
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public string StreetName { get; set; }
        public string ZoneLocalizedName { get; set; }

        public SpawnPoint(Vector3 position, Vector3 rotation, string streetName, string zoneLocalizedName)
        {
            Position = position;
            Rotation = rotation;
            StreetName = streetName;
            ZoneLocalizedName = zoneLocalizedName;
        }
    }
}
