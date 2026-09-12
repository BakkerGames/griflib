using static GrifLib.Common;

namespace GrifLib;

public partial class Dags
{
    /// <summary>
    /// Handle @for...@endfor
    /// </summary>
    private static void HandleFor(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        // @for(i,<start>,<end inclusive>)=...$i...@endfor
        var forStart = script.Index;
        var forEnd = 0;
        var level = 0;
        do
        {
            var token = script.Tokens[script.Index++];
            if (token.Equals(FOR_TOKEN, OIC))
            {
                level++;
            }
            else if (token.Equals(ENDFOR_TOKEN, OIC))
            {
                if (level <= 0)
                {
                    forEnd = script.Index - 1;
                    break;
                }
                level--;
            }
        } while (script.Index < script.Tokens.Length);
        if (forEnd == 0)
        {
            result.Add(new GrifMessage(MessageType.Error, $"{ENDFOR_TOKEN} not found"));
            return;
        }
        var int1 = long.Parse(p[1].Value);
        var int2 = long.Parse(p[2].Value);
        for (long value = int1; value <= int2; value++)
        {
            script.LocalData.Set($"{LOCAL_CHAR}{p[0].Value}", value);
            script.Index = forStart;
            do
            {
                result.AddRange(ProcessOneCommand(grod, script));
                if (script.BreakFlag)
                {
                    script.BreakFlag = false;
                    script.Index = forEnd + 1;
                    return;
                }
                if (script.ContinueFlag)
                {
                    script.ContinueFlag = false;
                    script.Index = forStart;
                    break;
                }
                if (script.ReturnFlag)
                {
                    return;
                }
                if (script.GoLabelFlag)
                {
                    if (script.Index < forStart || script.Index > forEnd)
                    {
                        // jumping out of block
                        return;
                    }
                }
            } while (script.Index < forEnd);
        }
        // skip @endfor
        script.Index = forEnd + 1;
    }

    /// <summary>
    /// Handle @foreachkey...@endforeachkey
    /// </summary>
    private static void HandleForEachKey(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        var foreachStart = script.Index;
        var foreachEnd = 0;
        var level = 0;
        do
        {
            var token = script.Tokens[script.Index++];
            if (token.Equals(FOREACHKEY_TOKEN, OIC))
            {
                level++;
            }
            else if (token.Equals(ENDFOREACHKEY_TOKEN, OIC))
            {
                if (level <= 0)
                {
                    foreachEnd = script.Index - 1;
                    break;
                }
                level--;
            }
        } while (script.Index < script.Tokens.Length);
        if (foreachEnd == 0)
        {
            result.Add(new GrifMessage(MessageType.Error, $"{ENDFOREACHKEY_TOKEN} not found"));
            return;
        }
        var keys = grod.Keys(p[1].Value, true, true);
        foreach (string key in keys)
        {
            var value = key[p[1].Value.Length..];
            if (p.Count > 2)
            {
                if (!value.EndsWith(p[2].Value, OIC))
                {
                    continue;
                }
                value = value[..^p[2].Value.Length];
            }
            script.LocalData.Set($"{LOCAL_CHAR}{p[0].Value}", value);
            script.Index = foreachStart;
            do
            {
                result.AddRange(ProcessOneCommand(grod, script));
                if (script.BreakFlag)
                {
                    script.BreakFlag = false;
                    script.Index = foreachEnd + 1;
                    return;
                }
                if (script.ContinueFlag)
                {
                    script.ContinueFlag = false;
                    script.Index = foreachStart;
                    break;
                }
                if (script.ReturnFlag)
                {
                    return;
                }
                if (script.GoLabelFlag)
                {
                    if (script.Index < foreachStart || script.Index > foreachEnd)
                    {
                        // jumping out of block
                        return;
                    }
                }
            } while (script.Index < foreachEnd);
        }
        // skip @endforeachkey
        script.Index = foreachEnd + 1;
    }

    /// <summary>
    /// Handle @foreachlist...@endforeachlist
    /// </summary>
    private static void HandleForEachList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        // @foreachlist(x,listname)=...$x...@endforeachlist
        var foreachlistStart = script.Index;
        var foreachlistEnd = 0;
        var level = 0;
        do
        {
            var token = script.Tokens[script.Index++];
            if (token.Equals(FOREACHLIST_TOKEN, OIC))
            {
                level++;
            }
            else if (token.Equals(ENDFOREACHLIST_TOKEN, OIC))
            {
                if (level <= 0)
                {
                    foreachlistEnd = script.Index - 1;
                    break;
                }
                level--;
            }
        } while (script.Index < script.Tokens.Length);
        if (foreachlistEnd == 0)
        {
            result.Add(new GrifMessage(MessageType.Error, $"{ENDFOREACHLIST_TOKEN} not found"));
            return;
        }
        // p[1] holds the name of the list
        var list = GetGlobalOrLocal(grod, script, p[1].Value, true);
        if (string.IsNullOrWhiteSpace(list))
        {
            return;
        }
        var items = SplitList(list);
        foreach (string item in items)
        {
            var value = FixListItemOut(item);
            script.LocalData.Set($"{LOCAL_CHAR}{p[0].Value}", value);
            script.Index = foreachlistStart;
            do
            {
                result.AddRange(ProcessOneCommand(grod, script));
                if (script.BreakFlag)
                {
                    script.BreakFlag = false;
                    script.Index = foreachlistEnd + 1;
                    return;
                }
                if (script.ContinueFlag)
                {
                    script.ContinueFlag = false;
                    script.Index = foreachlistStart;
                    break;
                }
                if (script.ReturnFlag)
                {
                    return;
                }
                if (script.GoLabelFlag)
                {
                    if (script.Index < foreachlistStart || script.Index > foreachlistEnd)
                    {
                        // jumping out of block
                        return;
                    }
                }
            } while (script.Index < foreachlistEnd);
        }
        // skip @endforeachlist
        script.Index = foreachlistEnd + 1;
    }
}
