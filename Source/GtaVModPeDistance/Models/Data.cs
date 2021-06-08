namespace GtaVModPeDistance.Models
{
    class Data
    {
        public double Distance { get; set; }
        public Ped2DBoundingBox BoundingBox { get; set; }
        public float PedHeight { get; set; }
        public float PedRotation { get; set; } //TODO: da normalizzare
        public float CameraHeightFromGround { get; set; }
        public string ImageName { get; set; }
        public string B64File { get; set; }
        public string DayTime { get; set; }

        public Data(double distance, Ped2DBoundingBox boundingBox, string imageName, string b64File, string dayTime, float pedRotation, float cameraHeightFromGround, float pedHeight)
        {
            Distance = distance;
            BoundingBox = boundingBox;
            ImageName = imageName;
            B64File = b64File;
            DayTime = dayTime;
            PedHeight = pedHeight;
            PedRotation = pedRotation;
            CameraHeightFromGround = cameraHeightFromGround;
        }
    }
}
