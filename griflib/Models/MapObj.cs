namespace griflib.Models;

public class MapObj
{
    public string? Key { get; set; }

    public long Height { get; set; }

    public long Width { get; set; }

    public long[,] Map { get; set; } = new long[0, 0];

    public MapObj()
    {
    }

    public MapObj(string key, long height, long width)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(key);
        ArgumentOutOfRangeException.ThrowIfNegative(height);
        ArgumentOutOfRangeException.ThrowIfNegative(width);
        Key = key;
        Height = height;
        Width = width;
    }
}
