using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.Models
{
    class ScreenShot
    {
        public string Name { get; set; }
        public string b64String { get; set; }
        public ScreenShot(string name, string b64String)
        {
            Name = name;
            this.b64String = b64String;
        }
    }
}
