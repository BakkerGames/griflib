namespace GrifLib;

public class ScriptObj
{
    public string? Name { get; set; }

    public string[] Tokens { get; set; } = [];

    public int Index { get; set; } = 0;

    public Grod LocalData { get; set; } = new();

    public bool ReturnFlag { get; set; } = false;

    public bool GoLabelFlag { get; set; } = false;

    public bool BreakFlag { get; set; } = false;

    public bool ContinueFlag { get; set; } = false;
}
