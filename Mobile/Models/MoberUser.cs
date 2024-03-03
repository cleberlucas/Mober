namespace Mobile.Models
{
    public class MoberUser
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Service { get; set; }
        public bool Servant { get; set; }
        public DateTime Updated { get; set; }
        public int Rate { get; set; }
    }
}
