using GTA;
using GTA.Math;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance
{
    class CoordinatesUtils
    {
        public static Vector3 playerPosition;

        public static Ped2DBoundingBox GetPedBoundingBoxInScreen(Ped ped)
        {
            Entity3DBoundingBox box3D = new Entity3DBoundingBox(ped);
            Vector2[] vertices = get3DBoundingBoxVertices(box3D);
            return get2DBoundingBoxFrom3DVertices(vertices);
        }

        private static Vector2[] get3DBoundingBoxVertices(Entity3DBoundingBox box3D)
        {
            Vector2[] vertices = new Vector2[8];
            vertices[0] = Utilities.World3DToScreen2d(box3D.TFR);
            vertices[1] = Utilities.World3DToScreen2d(box3D.TBL);
            vertices[2] = Utilities.World3DToScreen2d(box3D.TBR);
            vertices[3] = Utilities.World3DToScreen2d(box3D.TFL);
            vertices[4] = Utilities.World3DToScreen2d(box3D.DBL, box3D.TBL);
            vertices[5] = Utilities.World3DToScreen2d(box3D.DBR, box3D.TBR);
            vertices[6] = Utilities.World3DToScreen2d(box3D.DFR, box3D.TFR);
            vertices[7] = Utilities.World3DToScreen2d(box3D.DFL, box3D.TFL);
            return vertices;
        }

        private static Ped2DBoundingBox get2DBoundingBoxFrom3DVertices(Vector2[] vertices)
        {
            int xMin = int.MaxValue;
            int yMin = int.MaxValue;
            int xMax = 0;
            int yMax = 0;
            getBoundingBoxPoints(vertices, ref xMin, ref yMin, ref xMax, ref yMax);

            Vector2 BoundingBoxTopRight = new Vector2(xMax, yMin);
            Vector2 BoundingBoxBottomLeft = new Vector2(xMin, yMax);

            return new Ped2DBoundingBox(BoundingBoxBottomLeft, BoundingBoxTopRight);
        }

        private static void getBoundingBoxPoints(Vector2[] vertices, ref int xMin, ref int yMin, ref int xMax, ref int yMax)
        {
            foreach (Vector2 v2 in vertices)
            {
                int x = (int)v2.X;
                int y = (int)v2.Y;

                if (x < xMin)
                    xMin = x;
                if (x > xMax)
                    xMax = x;
                if (y < yMin)
                    yMin = y;
                if (y > yMax)
                    yMax = y;
            }

            if (xMin < 0)
                xMin = 0;
            if (yMin < 0)
                yMin = 0;
        }
    }
}
