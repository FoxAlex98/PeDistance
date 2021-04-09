using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.Models
{
    class Bones
    {
        public int Index { get; set; }
        public int BoneIndex { get; set; }
        public int HashCode { get; set; }
        public float BonesX { get; set; }
        public float BonesY { get; set; }
        public float BonesZ { get; set; }
        public Bones(int index, int boneIndex, int hashCode, float bonesX, float bonesY, float bonesZ)
        {
            Index = index;
            BoneIndex = BoneIndex;
            HashCode = hashCode;
            BonesX = bonesX;
            BonesY = bonesY;
            BonesZ = bonesZ;
        }
    }
}
