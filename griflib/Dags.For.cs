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
        var indexStart = script.Index;
        var indexEnd = 0;
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
                    indexEnd = script.Index - 1;
                    break;
                }
                level--;
            }
        } while (script.Index < script.Tokens.Length);
        if (indexEnd == 0)
        {
            result.Add(new GrifMessage(MessageType.Error, $"{ENDFOR_TOKEN} not found"));
            return;
        }
        var int1 = long.Parse(p[1].Value);
        var int2 = long.Parse(p[2].Value);
        for (long value = int1; value <= int2; value++)
        {
            script.LocalData.Set($"{LOCAL_CHAR}{p[0].Value}", value);
            script.Index = indexStart;
            do
            {
                var answer = ProcessOneCommand(grod, script);
                if (answer.Count > 0)
                {
                    result.AddRange(answer);
                }
                if (script.ReturnFlag)
                {
                    return;
                }
            } while (script.Index < indexEnd);
        }
        // skip @endfor
        script.Index = indexEnd + 1;
    }

    /// <summary>
    /// Handle @foreachkey...@endforeachkey
    /// </summary>
    private static void HandleForEachKey(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        var indexStart = script.Index;
        var indexEnd = 0;
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
                    indexEnd = script.Index - 1;
                    break;
                }
                level--;
            }
        } while (script.Index < script.Tokens.Length);
        if (indexEnd == 0)
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
            script.Index = indexStart;
            do
            {
                var answer = ProcessOneCommand(grod, script);
                if (answer.Count > 0)
                {
                    result.AddRange(answer);
                }
                if (script.ReturnFlag)
                {
                    return;
                }
            } while (script.Index < indexEnd);
        }
        // skip @endforeachkey
        script.Index = indexEnd + 1;
    }

    /// <summary>
    /// Handle @foreachlist...@endforeachlist
    /// </summary>
    private static void HandleForEachList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        // @foreachlist(x,listname)=...$x...@endforeachlist
        var indexStart = script.Index;
        var indexEnd = 0;
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
                    indexEnd = script.Index - 1;
                    break;
                }
                level--;
            }
        } while (script.Index < script.Tokens.Length);
        if (indexEnd == 0)
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
            script.Index = indexStart;
            do
            {
                var answer = ProcessOneCommand(grod, script);
                if (answer.Count > 0)
                {
                    result.AddRange(answer);
                }
                if (script.ReturnFlag)
                {
                    return;
                }
            } while (script.Index < indexEnd);
        }
        // skip @endforeachlist
        script.Index = indexEnd + 1;
    }
}
