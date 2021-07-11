using GTA.Math;

namespace GtaVModPeDistance.Models
{
    class SpawnPoint
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public string StreetName { get; set; }
        public string ZoneLocalizedName { get; set; }

        public SpawnPoint(float posX, float posY, float posZ, float rotX, float rotY, float rotZ, string streetName, string zoneLocalizedName)
        {
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            RotX = rotX;
            RotY = rotY;
            RotZ = rotZ;
            StreetName = streetName;
            ZoneLocalizedName = zoneLocalizedName;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(PosX, PosY, PosZ);
        }

        public Vector3 GetRotation()
        {
            return new Vector3(RotX, RotY, RotZ);
        }
    }
}
