using static GrifLib.Common;

namespace GrifLib;

public partial class Dags
{
    /// <summary>
    /// Error message for invalid @while syntax.
    /// </summary>
    private const string _invalidWhileSyntax = $"Invalid {WHILE_TOKEN} syntax";

    /// <summary>
    /// Process @while ... @do ... @endwhile block.
    /// </summary>
    private static List<GrifMessage> ProcessWhile(Grod grod, ScriptObj script)
    {
        // conditions
        List<GrifMessage> result = [];
        int whileStart = script.Index;
        SkipToEndWhile(script);
        var whileEnd = script.Index;
        bool whileOver = false;
        string token;
        while (!whileOver)
        {
            script.Index = whileStart;
            while (script.Index < script.Tokens.Length)
            {
                var cond = GetCondition(grod, script);
                if (script.ReturnFlag)
                {
                    return result;
                }
                if (script.Index >= script.Tokens.Length)
                {
                    throw new SystemException(_invalidWhileSyntax);
                }
                token = script.Tokens[script.Index++].ToLower();
                if (token == DO_TOKEN)
                {
                    if (!cond)
                    {
                        whileOver = true;
                        SkipToEndWhile(script);
                        if (script.ReturnFlag)
                        {
                            return result;
                        }
                        if (script.Tokens[script.Index].Equals(ENDWHILE_TOKEN, OIC))
                        {
                            script.Index++;
                            return result;
                        }
                        // @else
                        script.Index++;
                    }
                    break;
                }
                if (token == AND_TOKEN)
                {
                    if (!cond)
                    {
                        whileOver = true;
                        SkipOverDo(script);
                        if (script.ReturnFlag)
                        {
                            return result;
                        }
                        SkipToEndWhile(script);
                        if (script.ReturnFlag)
                        {
                            return result;
                        }
                        if (script.Tokens[script.Index].Equals(ENDWHILE_TOKEN, OIC))
                        {
                            script.Index++;
                            return result;
                        }
                        // @else
                        script.Index++;
                        break;
                    }
                }
                else if (token == OR_TOKEN)
                {
                    if (cond)
                    {
                        whileOver = true;
                        SkipOverEndWhile(script);
                        if (script.ReturnFlag)
                        {
                            return result;
                        }
                        break;
                    }
                }
                else
                {
                    throw new SystemException($"Unknown token in {WHILE_TOKEN}: {token}");
                }
            }
            // process all commands in this section
            while (script.Index < whileEnd)
            {
                result.AddRange(ProcessOneCommand(grod, script));
                if (script.BreakFlag)
                {
                    script.BreakFlag = false;
                    script.Index = whileEnd + 1;
                    return result;
                }
                if (script.ContinueFlag)
                {
                    script.ContinueFlag = false;
                    script.Index = whileStart;
                    break;
                }
                if (script.ReturnFlag)
                {
                    return result;
                }
                if (script.GoLabelFlag)
                {
                    if (script.Index < whileStart || script.Index > whileEnd)
                    {
                        // jumping out of block
                        return result;
                    }
                }
            }
        }
        throw new SystemException(_invalidWhileSyntax);
    }

    /// <summary>
    /// Skip over tokens until the matching @endwhile is found.
    /// </summary>
    private static void SkipOverEndWhile(ScriptObj script)
    {
        if (script.ReturnFlag)
        {
            return;
        }
        while (script.Index < script.Tokens.Length)
        {
            var token = script.Tokens[script.Index++];
            if (token.Equals(ENDWHILE_TOKEN, OIC))
            {
                return;
            }
            if (token.Equals(WHILE_TOKEN, OIC))
            {
                SkipOverEndWhile(script);
                if (script.ReturnFlag)
                {
                    return;
                }
            }
        }
        throw new SystemException($"Missing {ENDWHILE_TOKEN}");
    }

    /// <summary>
    /// Skip over tokens until the matching @do is found.
    /// </summary>
    private static void SkipOverDo(ScriptObj script)
    {
        if (script.ReturnFlag)
        {
            return;
        }
        while (script.Index < script.Tokens.Length)
        {
            var token = script.Tokens[script.Index++].ToLower();
            if (token == DO_TOKEN)
            {
                return;
            }
        }
        throw new SystemException($"Missing {DO_TOKEN}");
    }

    /// <summary>
    /// Skip to the next @endwhile token.
    /// </summary>
    private static void SkipToEndWhile(ScriptObj script)
    {
        if (script.ReturnFlag)
        {
            return;
        }
        while (script.Index < script.Tokens.Length)
        {
            var token = script.Tokens[script.Index].ToLower();
            if (token == ENDWHILE_TOKEN)
            {
                return;
            }
            script.Index++;
            if (token.Equals(WHILE_TOKEN, OIC))
            {
                SkipOverEndWhile(script);
                if (script.ReturnFlag)
                {
                    return;
                }
            }
        }
        throw new SystemException(_invalidWhileSyntax);
    }
}
