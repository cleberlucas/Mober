namespace Mobile.Models;

public class Point
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }


    private Passenger Passenger = new Passenger();
    public string Period { get => Passenger.Period; set => Passenger.Period = value; }

}
