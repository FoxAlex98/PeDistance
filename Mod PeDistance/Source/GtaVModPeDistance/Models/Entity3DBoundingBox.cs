using GTA;
using GTA.Math;

namespace GtaVModPeDistance.Models
{
    class Entity3DBoundingBox
    {
       
        public Vector3 DBL { get; set; }
        public Vector3 DBR { get; set; }
        public Vector3 DFR { get; set; }
        public Vector3 DFL { get; set; }
        public Vector3 TFR { get; set; }
        public Vector3 TBL { get; set; }
        public Vector3 TBR { get; set; }
        public Vector3 TFL { get; set; }

        public Entity3DBoundingBox(Entity entity,
            float DBLXOffset = 0.25f, float DBLYOffset = 0f, float DBLZOffset = 0.2f,
            float TFRXOffset = -0.25f, float TFRYOffset = 0f, float TFRZOffset = 0f)
        {

            Vector3 previousRotation = entity.Rotation;
            entity.Rotation = new Vector3(previousRotation.X, previousRotation.Y, 0);

            DBL = entity.Model.Dimensions.rearBottomLeft;     // Down Behind left
            TFR = entity.Model.Dimensions.frontTopRight;      // Top front right

            // aggiustamenti dei margini
            DBL = new Vector3(DBL.X + DBLXOffset, DBL.Y + DBLYOffset, DBL.Z + DBLZOffset);
            TFR = new Vector3(TFR.X + TFRXOffset, TFR.Y + TFRYOffset, TFR.Z + TFRZOffset);

            DBR = new Vector3(TFR.X, DBL.Y, DBL.Z);        // Down Behind right
            DFR = new Vector3(TFR.X, TFR.Y, DBL.Z);        // Down front right
            DFL = new Vector3(DBL.X, TFR.Y, DBL.Z);        // Down front left

            TBL = new Vector3(DBL.X, DBL.Y, TFR.Z);        // Top Behind left
            TBR = new Vector3(TFR.X, DBL.Y, TFR.Z);        // Top Behind right
            TFL = new Vector3(DBL.X, TFR.Y, TFR.Z);        // Top front left

            entity.Rotation = previousRotation;

            DBL = entity.GetOffsetPosition(DBL);
            DBR = entity.GetOffsetPosition(DBR);
            DFR = entity.GetOffsetPosition(DFR);
            DFL = entity.GetOffsetPosition(DFL);

            TFR = entity.GetOffsetPosition(TFR);
            TBL = entity.GetOffsetPosition(TBL);
            TBR = entity.GetOffsetPosition(TBR);
            TFL = entity.GetOffsetPosition(TFL);
        }
    }
}
