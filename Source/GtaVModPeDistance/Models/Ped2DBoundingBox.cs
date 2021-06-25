using GTA.Math;

namespace GtaVModPeDistance.Models
{
    class Ped2DBoundingBox
    {
        public float PedBottomLeftX { get; set; }
        public float PedBottomLeftY { get; set; }
        public float PedTopRightX { get; set; }
        public float PedTopRightY { get; set; }
        public Ped2DBoundingBox(Vector2 pedBottomLeft, Vector2 pedTopRight)
        {
            PedBottomLeftX = pedBottomLeft.X;
            PedBottomLeftY = pedBottomLeft.Y;
            PedTopRightX = pedTopRight.X;
            PedTopRightY = pedTopRight.Y;
        }
    }
}
