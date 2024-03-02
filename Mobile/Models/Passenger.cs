namespace Mobile.Models;

public class Passenger
{
    public Associate Associate { get; set; }
    public DateTime DateTime { get; set; }
    public string Period { get; set; }
    public Point Point { get; set; }
    public string PresenceGone { get; set; }
    public string PresenceBack { get; set; }

 
    private Color VisualColor;
    public Color Color { get => VisualColor; set => VisualColor = value; }

}
