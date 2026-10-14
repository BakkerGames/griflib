namespace griflib.Models;

public class MapObj
{
    public long Height { get; set; }

    public long Width { get; set; }

    public long[,] Map { get; set; } = new long[0, 0];
}
