using System.Globalization;
using System.Text;
using static GrifLib.Common;

namespace GrifLib;

public partial class Dags
{
    public static void Exec_Abs(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(p[0].Value);
        if (long1 < 0)
        {
            long1 = -long1;
        }
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_Add(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        var longAnswer = long1 + long2;
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_AddList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        AddListItem(grod, script, p[0].Value, p[1].Value);
    }

    public static void Exec_AddTo(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(GetGlobalOrLocal(grod, script, p[0].Value, true));
        var long2 = GetNumberValue(p[1].Value);
        var longAnswer = long1 + long2;
        SetGlobalOrLocal(grod, script, p[0].Value, longAnswer.ToString());
    }

    public static void Exec_BitwiseAnd(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0)
        {
            throw new SystemException($"{BITWISEAND_TOKEN}{long1},{long2}): Parameters cannot be negative");
        }
        var longAnswer = long1 & long2;
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_BitwiseOr(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0)
        {
            throw new SystemException($"{BITWISEOR_TOKEN}{long1},{long2}): Parameters cannot be negative");
        }
        var longAnswer = long1 | long2;
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_BitwiseXor(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0)
        {
            throw new SystemException($"{BITWISEXOR_TOKEN}{long1},{long2}): Parameters cannot be negative");
        }
        var longAnswer = long1 ^ long2;
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_ClearArray(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        ClearArray(grod, script, p[0].Value);
    }

    public static void Exec_ClearBit(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0 || long2 > 30)
        {
            throw new SystemException($"{CLEARBIT_TOKEN}{long1},{long2}): Invalid parameters");
        }
        var longAnswer = long1 ^ (long)Math.Pow(2, long2);
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_ClearList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        if (string.IsNullOrWhiteSpace(p[0].Value))
        {
            throw new SystemException($"{CLEARLIST_TOKEN}): List name cannot be blank");
        }
        SetGlobalOrLocal(grod, script, p[0].Value, null);
    }

    public static void Exec_Comment(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        // Ignore comments
    }

    public static void Exec_Concat(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterAtLeastOne(p);
        StringBuilder sb = new();
        foreach (var item in p)
        {
            sb.Append(item.Value);
        }
        result.Add(new GrifMessage(MessageType.Internal, sb.ToString()));
    }

    public static void Exec_Contains(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var list1 = SplitList(p[0].Value);
        if (list1.Contains(p[1].Value))
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
            return;
        }
        result.Add(new GrifMessage(MessageType.Internal, FALSE));
    }

    public static void Exec_ContainsList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var list1 = SplitList(p[0].Value);
        var list2 = SplitList(p[1].Value);
        if (list1.Length == 0 || list2.Length == 0)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
            return;
        }
        foreach (var listItem in list2)
        {
            if (!list1.Contains(listItem))
            {
                result.Add(new GrifMessage(MessageType.Internal, FALSE));
                break;
            }
        }
        result.Add(new GrifMessage(MessageType.Internal, TRUE));
    }

    public static void Exec_DateTime(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParmeterCountBetween(p, 1, 2);
        try
        {
            if (p.Count == 2)
            {
                if (p[1].Value.Equals("UTC", OIC))
                {
                    var dt = DateTime.UtcNow.ToString(p[0].Value, CultureInfo.InvariantCulture);
                    result.Add(new GrifMessage(MessageType.Internal, dt, "UTC"));
                }
                else
                {
                    throw new SystemException($"Unknown time zone parameter {p[1].Value}");
                }
            }
            else
            {
                var dt = DateTime.Now.ToString(p[0].Value, CultureInfo.InvariantCulture);
                var tz = TimeZoneInfo.Local.DisplayName[4..10]; // "-05:00"
                result.Add(new GrifMessage(MessageType.Internal, dt, tz));
            }
        }
        catch (Exception)
        {
            throw new SystemException($"Error converting date format {p[0].Value}");
        }
    }

    public static void Exec_Debug(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        AddDebugMessage(grod, result, p[0].Value);
    }

    public static void Exec_Div(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long2 == 0)
        {
            throw new SystemException("Division by zero is not allowed.");
        }
        long1 /= long2;
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_DivTo(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(GetGlobalOrLocal(grod, script, p[0].Value, true));
        var long2 = GetNumberValue(p[1].Value);
        if (long2 == 0)
        {
            throw new SystemException("Division by zero is not allowed.");
        }
        long1 /= long2;
        SetGlobalOrLocal(grod, script, p[0].Value, long1.ToString());
    }

    public static void Exec_Equals(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var isNull0 = IsNull(p[0].Value);
        var isNull1 = IsNull(p[1].Value);
        if (isNull0 && isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (isNull0 || isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (string.Compare(p[0].Value, p[1].Value, OIC) == 0)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (long.TryParse(p[0].Value, out long long1) && long.TryParse(p[1].Value, out long long2))
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(long1 == long2)));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
    }

    public static void Exec_Exec(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var value = p[0].Value;
        result.AddRange(Process(grod, value));
    }

    public static void Exec_Exists(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var value = GetGlobalOrLocal(grod, script, p[0].Value, true);
        result.Add(new GrifMessage(MessageType.Internal, TrueFalse(!IsNullOrEmpty(value))));
    }

    public static void Exec_Flipbit(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0 || long2 > 30)
        {
            throw new SystemException($"{FLIPBIT_TOKEN}{long1},{long2}): Invalid parameters");
        }
        var longAnswer = long1 ^ (long)Math.Pow(2, long2);
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_For(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 3);
        HandleFor(grod, script, p, result);
    }

    public static void Exec_ForEachKey(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        if (p.Count != 2 && p.Count != 3)
        {
            CheckParameterCount(p, 3);
        }
        HandleForEachKey(grod, script, p, result);
    }

    public static void Exec_ForEachList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        HandleForEachList(grod, script, p, result);
    }

    public static void Exec_Format(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterAtLeastOne(p);
        var value = p[0].Value;
        for (int i = 1; i < p.Count; i++)
        {
            value = value.Replace($"{{{i - 1}}}", p[i].Value); // {0} = p[1]
        }
        result.Add(new GrifMessage(MessageType.Internal, value));
    }

    public static void Exec_FromBinary(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        try
        {
            var long1 = Convert.ToInt64(p[0].Value, 2);
            result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
        }
        catch (Exception)
        {
            throw new SystemException($"{FROMBINARY_TOKEN}{p[0].Value}): Invalid binary string");
        }
    }

    public static void Exec_FromHex(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        try
        {
            var long1 = Convert.ToInt64(p[0].Value, 16);
            var value = long1.ToString();
            result.Add(new GrifMessage(MessageType.Internal, value));
        }
        catch (Exception)
        {
            throw new SystemException($"{FROMHEX_TOKEN}{p[0].Value}): Invalid hex string");
        }
    }

    public static void Exec_Get(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var value = GetGlobalOrLocal(grod, script, p[0].Value, true) ?? "";
        result.Add(new GrifMessage(MessageType.Internal, value));
    }

    public static void Exec_GetArray(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 3);
        if (string.IsNullOrWhiteSpace(p[0].Value))
        {
            throw new SystemException("Array name cannot be blank");
        }
        var long1 = GetNumberValue(p[1].Value);
        var long2 = GetNumberValue(p[2].Value);
        var value = GetArrayItem(grod, script, p[0].Value, long1, long2);
        result.Add(new GrifMessage(MessageType.Internal, value ?? ""));
    }

    public static void Exec_GetBit(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0 || long2 > 30)
        {
            throw new SystemException($"{GETBIT_TOKEN}{long1},{long2}): Invalid parameters");
        }
        var longAnswer = long1 & (long)Math.Pow(2, long2);
        if (longAnswer != 0)
        {
            longAnswer = 1;
        }
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_GetChar(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var int1 = (int)GetNumberValue(p[1].Value);
        if (int1 < 0)
        {
            throw new SystemException("Index out of range");
        }
        if (int1 >= p[0].Value.Length)
        {
            result.Add(new GrifMessage(MessageType.Internal, " "));
        }
        else
        {
            var value = p[0].Value.Substring(int1, 1);
            result.Add(new GrifMessage(MessageType.Internal, value));
        }
    }

    public static void Exec_GetInChannel(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        result.Add(new GrifMessage(MessageType.Internal, grod.Get(INCHANNEL, true) ?? ""));
        grod.Remove(INCHANNEL, false); // Clear after reading
    }

    public static void Exec_GetList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        if (string.IsNullOrWhiteSpace(p[0].Value))
        {
            throw new SystemException("List name cannot be blank");
        }
        var long1 = GetNumberValue(p[1].Value);
        var value = GetListItem(grod, script, p[0].Value, long1);
        result.Add(new GrifMessage(MessageType.Internal, value ?? ""));
    }

    public static void Exec_GetValue(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var value = GetValue(grod, GetGlobalOrLocal(grod, script, p[0].Value, true));
        result.Add(new GrifMessage(MessageType.Internal, value));
    }

    public static void Exec_GoLabel(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        for (int i = 0; i < script.Tokens.Length - 1; i++)
        {
            if (script.Tokens[i] == LABEL_TOKEN
                && script.Tokens[i + 1] == p[0].Value
                && script.Tokens[i + 2] == ")")
            {
                script.Index = i + 3;
            }
        }
    }

    public static void Exec_GreaterThan(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var isNull0 = IsNull(p[0].Value);
        var isNull1 = IsNull(p[1].Value);
        if (!isNull0 && isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (isNull0)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (long.TryParse(p[0].Value, out long long1) && long.TryParse(p[1].Value, out long long2))
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(long1 > long2)));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal,
                TrueFalse(string.Compare(p[0].Value, p[1].Value, OIC) > 0)));
        }
    }

    public static void Exec_GreaterThanOrEquals(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var isNull0 = IsNull(p[0].Value);
        var isNull1 = IsNull(p[1].Value);
        if (isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (isNull0)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (long.TryParse(p[0].Value, out long long1) && long.TryParse(p[1].Value, out long long2))
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(long1 >= long2)));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal,
                TrueFalse(string.Compare(p[0].Value, p[1].Value, OIC) >= 0)));
        }
    }

    public static void Exec_If(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        result.AddRange(ProcessIf(grod, script));
    }

    public static void Exec_InList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        if (InList(grod, script, p[0].Value, p[1].Value))
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
    }

    public static void Exec_InsertAtList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 3);
        var long1 = GetNumberValue(p[1].Value);
        InsertAtListItem(grod, script, p[0].Value, long1, p[2].Value);
    }

    public static void Exec_IsBool(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        try
        {
            var boolAnswer = IsTrue(p[0].Value);
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        catch (Exception)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
    }

    public static void Exec_IsFalse(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        try
        {
            var boolAnswer = IsTrue(p[0].Value);
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(!boolAnswer)));
        }
        catch (Exception)
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(false)));
        }
    }

    public static void Exec_IsNull(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        result.Add(new GrifMessage(MessageType.Internal, TrueFalse(IsNullOrEmpty(p[0].Value))));
    }

    public static void Exec_IsNumber(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        if (long.TryParse(p[0].Value, out _))
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
    }

    public static void Exec_IsScript(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var value = GetGlobalOrLocal(grod, script, p[0].Value, true);
        if (IsNull(value))
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(IsScript(value))));
        }
    }

    public static void Exec_IsTrue(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        try
        {
            var boolAnswer = IsTrue(p[0].Value);
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(boolAnswer)));
        }
        catch (Exception)
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(false)));
        }
    }

    public static void Exec_Label(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
    }

    public static void Exec_Len(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        result.Add(new GrifMessage(MessageType.Internal, p[0].Value.Length.ToString()));
    }

    public static void Exec_LessThan(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var isNull0 = IsNull(p[0].Value);
        var isNull1 = IsNull(p[1].Value);
        if (isNull0 && !isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (long.TryParse(p[0].Value, out long long1) && long.TryParse(p[1].Value, out long long2))
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(long1 < long2)));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal,
                TrueFalse(string.Compare(p[0].Value, p[1].Value, OIC) < 0)));
        }
    }

    public static void Exec_LessThanOrEquals(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var isNull0 = IsNull(p[0].Value);
        var isNull1 = IsNull(p[1].Value);
        if (isNull0)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (long.TryParse(p[0].Value, out long long1) && long.TryParse(p[1].Value, out long long2))
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(long1 <= long2)));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal,
                TrueFalse(string.Compare(p[0].Value, p[1].Value, OIC) <= 0)));
        }
    }

    public static void Exec_ListLength(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        if (string.IsNullOrWhiteSpace(p[0].Value))
        {
            throw new SystemException($"{LISTLENGTH_TOKEN}): List name cannot be blank");
        }
        var value = GetGlobalOrLocal(grod, script, p[0].Value, true);
        if (IsNullOrEmpty(value))
        {
            result.Add(new GrifMessage(MessageType.Internal, "0"));
        }
        else
        {
            var list1 = SplitList(value);
            result.Add(new GrifMessage(MessageType.Internal, list1.Length.ToString()));
        }
    }

    public static void Exec_Lower(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        result.Add(new GrifMessage(MessageType.Internal, p[0].Value.ToLower()));
    }

    public static void Exec_Max(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        long1 = Math.Max(long1, long2);
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_Min(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        long1 = Math.Min(long1, long2);
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_Mod(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        long1 %= long2;
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_ModTo(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(GetGlobalOrLocal(grod, script, p[0].Value, true));
        var long2 = GetNumberValue(p[1].Value);
        long1 %= long2;
        SetGlobalOrLocal(grod, script, p[0].Value, long1.ToString());
    }

    public static void Exec_Msg(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var tempResult = Process(grod, GetGlobalOrLocal(grod, script, p[0].Value, true));
        foreach (var msgItem in tempResult)
        {
            if (msgItem.Type == MessageType.Text || msgItem.Type == MessageType.Internal)
            {
                var value = msgItem.Value;
                if (!string.IsNullOrEmpty(value)) // whitespace is allowed
                {
                    result.Add(new GrifMessage(MessageType.Text, value));
                }
            }
        }
        result.Add(new GrifMessage(MessageType.Text, NL_CHAR));
    }

    public static void Exec_Mul(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        long1 *= long2;
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_MulTo(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(GetGlobalOrLocal(grod, script, p[0].Value, true));
        var long2 = GetNumberValue(p[1].Value);
        long1 *= long2;
        SetGlobalOrLocal(grod, script, p[0].Value, long1.ToString());
    }

    public static void Exec_Neg(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(p[0].Value);
        long1 = -long1;
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_NegTo(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(GetGlobalOrLocal(grod, script, p[0].Value, true));
        long1 = -long1;
        SetGlobalOrLocal(grod, script, p[0].Value, long1.ToString());
    }

    public static void Exec_Newline(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        result.Add(new GrifMessage(MessageType.Text, NL_CHAR));
    }

    public static void Exec_NotEqual(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var isNull0 = IsNull(p[0].Value);
        var isNull1 = IsNull(p[1].Value);
        if (isNull0 && isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (isNull0 || isNull1)
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
        else if (string.Compare(p[0].Value, p[1].Value, OIC) == 0)
        {
            result.Add(new GrifMessage(MessageType.Internal, FALSE));
        }
        else if (long.TryParse(p[0].Value, out long long1) && long.TryParse(p[1].Value, out long long2))
        {
            result.Add(new GrifMessage(MessageType.Internal, TrueFalse(long1 != long2)));
        }
        else
        {
            result.Add(new GrifMessage(MessageType.Internal, TRUE));
        }
    }

    public static void Exec_OnGoLabel(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterAtLeastOne(p);
        var int1 = (int)GetNumberValue(p[0].Value);
        if (int1 > 0 && int1 < p.Count) // else fall through
        {
            var value = p[int1].Value;
            for (int i = 0; i < script.Tokens.Length - 1; i++)
            {
                if (script.Tokens[i] == LABEL_TOKEN
                    && script.Tokens[i + 1] == value
                    && script.Tokens[i + 2] == ")")
                {
                    script.Index = i + 3;
                }
            }
        }
    }

    public static void Exec_Rand(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = _random.Next(100);
        var boolAnswer = long2 < long1;
        result.Add(new GrifMessage(MessageType.Internal, TrueFalse(boolAnswer)));
    }

    public static void Exec_RemoveAtList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[1].Value);
        RemoveAtListItem(grod, script, p[0].Value, long1);
    }

    public static void Exec_Replace(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 3);
        result.Add(new GrifMessage(MessageType.Internal, p[0].Value.Replace(p[1].Value, p[2].Value, OIC)));
    }

    public static void Exec_Return(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        script.Index = script.Tokens.Length;
        script.ReturnFlag = true; // End processing
    }

    public static void Exec_Rnd(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(p[0].Value);
        result.Add(new GrifMessage(MessageType.Internal, _random.Next((int)long1).ToString()));
    }

    public static void Exec_Script(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var value = GetGlobalOrLocal(grod, script, p[0].Value, true);
        if (!string.IsNullOrWhiteSpace(value))
        {
            var scriptResult = Process(grod, value);
            result.AddRange(scriptResult);
        }
    }

    public static void Exec_Set(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        SetGlobalOrLocal(grod, script, p[0].Value, p[1].Value);
    }

    public static void Exec_SetArray(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 4);
        if (string.IsNullOrWhiteSpace(p[0].Value))
        {
            throw new SystemException("Array name cannot be blank");
        }
        var long1 = GetNumberValue(p[1].Value); // y
        var long2 = GetNumberValue(p[2].Value); // x
        SetArrayItem(grod, script, p[0].Value, long1, long2, p[3].Value);
    }

    public static void Exec_SetBit(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        if (long1 < 0 || long2 < 0 || long2 > 30)
        {
            throw new SystemException($"{SETBIT_TOKEN}{long1},{long2}): Invalid parameters");
        }
        var longAnswer = long1 | (long)Math.Pow(2, long2);
        result.Add(new GrifMessage(MessageType.Internal, longAnswer.ToString()));
    }

    public static void Exec_SetChar(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 3);
        var int1 = (int)GetNumberValue(p[1].Value);
        if (int1 < 0)
        {
            throw new SystemException("Index out of range");
        }
        if (p[2].Value == null || p[2].Value == NULL || p[2].Value.Length < 1)
        {
            throw new SystemException("Character not supplied");
        }
        var value = p[0].Value;
        if (int1 == value.Length)
        {
            value += p[2].Value[0];
        }
        else if (int1 > value.Length)
        {
            value += new string(' ', int1 - value.Length - 1) + p[2].Value[0];
        }
        else
        {
            value = value[..int1] + p[2].Value[0] + value[(int1 + 1)..];
        }
        result.Add(new GrifMessage(MessageType.Internal, value));
    }

    public static void Exec_SetExtra(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        // Set a value in a parent grod by name.
        // Good for configuration settings or notes or global variables across all saves.
        // Use OUTCHANNEL_ADD_EXTRA_GROD first to create a parent grod with a name.
        CheckParameterCount(p, 3);
        result.Add(new GrifMessage(MessageType.OutChannel, OUTCHANNEL_SET_EXTRA_VALUE,
            p[0].Value + '\t' + p[1].Value + '\t' + p[2].Value));
    }

    public static void Exec_SetList(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 3);
        var long1 = GetNumberValue(p[1].Value);
        SetListItem(grod, script, p[0].Value, long1, p[2].Value);
    }

    public static void Exec_SetOutChannel(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        if (p.Count == 1)
        {
            result.Add(new GrifMessage(MessageType.OutChannel, p[0].Value));
        }
        else if (p.Count == 2)
        {
            result.Add(new GrifMessage(MessageType.OutChannel, p[0].Value, p[1].Value));
        }
    }

    public static void Exec_Sub(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(p[0].Value);
        var long2 = GetNumberValue(p[1].Value);
        long1 -= long2;
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString()));
    }

    public static void Exec_Substring(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParmeterCountBetween(p, 2, 3);
        var long1 = GetNumberValue(p[1].Value);
        long long2;
        if (p.Count == 2)
        {
            long2 = p[0].Value.Length - long1;
        }
        else
        {
            long2 = GetNumberValue(p[2].Value);
        }
        if (long1 < 0 || long2 < 0 || long1 + long2 > p[0].Value.Length)
        {
            throw new SystemException($"{SUBSTRING_TOKEN}{p[0].Value},{long1},{long2}): Invalid parameters");
        }
        result.Add(new GrifMessage(MessageType.Internal, p[0].Value.Substring((int)long1, (int)long2)));
    }

    public static void Exec_SubTo(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var long1 = GetNumberValue(GetGlobalOrLocal(grod, script, p[0].Value, true));
        var long2 = GetNumberValue(p[1].Value);
        long1 -= long2;
        SetGlobalOrLocal(grod, script, p[0].Value, long1.ToString());
    }

    public static void Exec_Swap(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 2);
        var value1 = GetGlobalOrLocal(grod, script, p[0].Value, true);
        var value2 = GetGlobalOrLocal(grod, script, p[1].Value, true);
        SetGlobalOrLocal(grod, script, p[0].Value, value2);
        SetGlobalOrLocal(grod, script, p[1].Value, value1);
    }

    public static void Exec_ToBinary(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(p[0].Value);
        result.Add(new GrifMessage(MessageType.Internal, Convert.ToString(long1, 2)));
    }

    public static void Exec_ToHex(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        var long1 = GetNumberValue(p[0].Value);
        result.Add(new GrifMessage(MessageType.Internal, long1.ToString("X")));
    }

    public static void Exec_Trim(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        result.Add(new GrifMessage(MessageType.Internal, p[0].Value.Trim()));
    }

    public static void Exec_Upper(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterCount(p, 1);
        result.Add(new GrifMessage(MessageType.Internal, p[0].Value.ToUpper()));
    }

    public static void Exec_While(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        result.AddRange(ProcessWhile(grod, script));
    }

    public static void Exec_Write(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterAtLeastOne(p);
        foreach (var item in p) // concatenate all parameters
        {
            var value = GetValue(grod, item.Value);
            result.Add(new GrifMessage(MessageType.Text, value, item.ExtraValue));
        }
    }

    public static void Exec_WriteLine(Grod grod, ScriptObj script, List<GrifMessage> p, List<GrifMessage> result)
    {
        CheckParameterAtLeastOne(p);
        foreach (var item in p) // concatenate all parameters
        {
            var value = GetValue(grod, item.Value);
            result.Add(new GrifMessage(MessageType.Text, value, item.ExtraValue));
        }
        result.Add(new GrifMessage(MessageType.Text, NL_CHAR));
    }
}
