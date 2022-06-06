using System.Collections.Generic;
public class Sonar
{
    public int no_pings { get; set; }
    public int no_counts { get; set; }
    public int minimum_depth { get; set; }
    public int maximum_depth { get; set; }
    public int min_length_axis { get; set; }
    public int max_length_axis { get; set; }
    public int min_width_axis { get; set; }
    public int max_width_axis { get; set; }
    public List<Ping> pings { get; set; }
}