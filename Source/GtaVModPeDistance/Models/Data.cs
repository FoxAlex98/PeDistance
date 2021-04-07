using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.Models
{
    class Data
    {
        public int id { get; set; }
        public double Distance { get; set; }
        public float PedHeigth { get; set; }
        public float PedRotation { get; set; }
        public float CameraHeigthFromGround { get; set; }
        public string ImageName { get; set; }
        public string B64File { get; set; }
        public string DayTime { get; set; }

        public Data(int id, double distance, float pedHeigth, float pedRotation, float cameraHeigthFromGround, string imageName, string b64File, string dayTime)
        {
            this.id = id;
            Distance = distance;
            PedHeigth = pedHeigth;
            PedRotation = pedRotation;
            CameraHeigthFromGround = cameraHeigthFromGround;
            ImageName = imageName;
            B64File = b64File;
            DayTime = dayTime;
        }
    }
}
