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
