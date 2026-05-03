using static GrifLib.Common;

namespace GrifLib;


public partial class Dags
{
    private const string helpResource = "griflib.help.grif";

    /// <summary>
    /// Return the contents of helpResource as a GROD.
    /// </summary>
    public static Grod? Help()
    {
        var helpGrif = IO.GetResourceStream(helpResource);
        if (helpGrif == null)
        {
            return null;
        }
        var result = new Grod("help");
        result.AddItems(IO.ReadGrif(new StreamReader(helpGrif)));
        return result;
    }

    /// <summary>
    /// Return the contents of helpResource where the key or value contains searchTerm.
    /// </summary>
    public static Grod? Help(string searchTerm)
    {
        var grod = Help();
        if (grod == null)
        {
            return null;
        }

        var result = new Grod("helpsearch");

        foreach (var item in grod.Items(true, true))
        {
            if (item.Key.Contains(searchTerm, OIC)
                || (item.Value ?? "").Contains(searchTerm, OIC))
            {
                result.Set(item.Key, item.Value);
            }
        }

        return result;
    }
}
