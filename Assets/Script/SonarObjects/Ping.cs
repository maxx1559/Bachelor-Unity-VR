using System.Collections.Generic;

public class Ping
{
    public int pingID { get; set; }
    public int no_points { get; set; }
    public List<double> ping_boat_coord { get; set; }
    public List<double> ping_coord { get; set; }
    public List<double> coords_x { get; set; }
    public List<double> coords_y { get; set; }
    public List<double> coords_z { get; set; }
}
