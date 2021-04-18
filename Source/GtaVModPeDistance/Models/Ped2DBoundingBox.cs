using GTA.Math;

namespace GtaVModPeDistance.Models
{
    class Ped2DBoundingBox
    {
        public float PedTopLeftX { get; set; }
        public float PedTopLeftY { get; set; }
        public float PedTopRightX { get; set; }
        public float PedTopRightY { get; set; }
        public float PedBottomLeftX { get; set; }
        public float PedBottomLeftY { get; set; }
        public float PedBottomRightX { get; set; }
        public float PedBottomRightY { get; set; }
        public Ped2DBoundingBox(Vector2 pedTopLeft, Vector2 pedTopRight, Vector2 pedBottomLeft, Vector2 pedBottomRight)
        {
            PedTopLeftX = pedTopLeft.X;
            PedTopLeftY = pedTopLeft.Y;
            PedTopRightX = pedTopRight.X;
            PedTopRightY = pedTopRight.Y;
            PedBottomLeftX = pedBottomLeft.X;
            PedBottomLeftY = pedBottomLeft.Y;
            PedBottomRightX = pedBottomRight.X;
            PedBottomRightY = pedBottomRight.Y;
        }
    }
}
