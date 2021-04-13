namespace GtaVModPeDistance.Models
{
    class Data
    {
        public int id { get; set; }
        public double Distance { get; set; }
        public Ped2DBoundingBox BoundingBox { get; set; }
        public float PedHeight { get; set; }
        public float PedRotation { get; set; } //TODO: da normalizzare
        public float CameraHeightFromGround { get; set; }
        public string ImageName { get; set; }
        public string B64File { get; set; }
        public string DayTime { get; set; }

        public Data(int id, double distance, Ped2DBoundingBox boundingBox, float pedHeight, float pedRotation, float cameraHeightFromGround, string imageName, string b64File, string dayTime)
        {
            this.id = id;
            Distance = distance;
            BoundingBox = boundingBox;
            PedHeight = pedHeight;
            PedRotation = pedRotation;
            CameraHeightFromGround = cameraHeightFromGround;
            ImageName = imageName;
            B64File = b64File;
            DayTime = dayTime;
        }
    }
}
