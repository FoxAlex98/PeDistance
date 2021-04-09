using GTA.Math;
using CsvHelper.Configuration.Attributes;

namespace GtaVModPeDistance.Models
{
    class SpawnPoint
    {
        //[Name("posX")]
        public float PosX { get; set; }
        //[Name("posY")]
        public float PosY { get; set; }
        //[Name("posZ")]
        public float PosZ { get; set; }
        //[Name("rotX")]
        public float RotX { get; set; }
        //[Name("rotY")]
        public float RotY { get; set; }
        //[Name("rotZ")]
        public float RotZ { get; set; }
        //[Name("streetName")]
        public string StreetName { get; set; }
        //[Name("zoneLocalizedName")]
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
