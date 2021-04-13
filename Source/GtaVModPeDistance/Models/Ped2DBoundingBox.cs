using GTA.Math;

namespace GtaVModPeDistance.Models
{
    class Ped2DBoundingBox
    {
        public Vector2 PedTopLeft { get; set; }
        public Vector2 PedTopRight { get; set; }
        public Vector2 PedBottomLeft { get; set; }
        public Vector2 PedBottomRight { get; set; }
        public Ped2DBoundingBox(Vector2 pedTopLeft, Vector2 pedTopRight, Vector2 pedBottomLeft, Vector2 pedBottomRight)
        {
            PedTopLeft = pedTopLeft;
            PedTopRight = pedTopRight;
            PedBottomLeft = pedBottomLeft;
            PedBottomRight = pedBottomRight;
        }
    }
}
