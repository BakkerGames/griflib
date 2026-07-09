using System.Text;
using static GrifLib.Common;

namespace GrifLib;

public partial class Dags
{
    private static readonly List<GrifMessage> pEmpty = [];

    /// <summary>
    /// Process one command from the token list.
    /// </summary>
    private static List<GrifMessage> ProcessOneCommand(Grod grod, ScriptObj script)
    {
        List<GrifMessage> result = [];
        string? value;
        try
        {
            if (script.Index >= script.Tokens.Length)
            {
                return result;
            }
            if (script.ReturnFlag)
            {
                script.Index = script.Tokens.Length;
                return result;
            }
            var token = script.Tokens[script.Index++];
            // static value
            if (!token.StartsWith(SCRIPT_CHAR))
            {
                result.Add(new GrifMessage(MessageType.Internal, token));
                return result;
            }
            // script.Tokens without parameters
            if (!token.EndsWith('('))
            {
                switch (token.ToLower())
                {
                    case IF_TOKEN:
                        Exec_If(grod, script, pEmpty, result);
                        break;
                    case GETINCHANNEL_TOKEN:
                        Exec_GetInChannel(grod, script, pEmpty, result);
                        break;
                    case NL_TOKEN:
                        Exec_Newline(grod, script, pEmpty, result);
                        break;
                    case RETURN_TOKEN:
                        Exec_Return(grod, script, pEmpty, result);
                        break;
                    case WHILE_TOKEN:
                        Exec_While(grod, script, pEmpty, result);
                        break;
                    case AND_TOKEN:
                    case DO_TOKEN:
                    case ELSEIF_TOKEN:
                    case ELSE_TOKEN:
                    case ENDFOREACHKEY_TOKEN:
                    case ENDFOREACHLIST_TOKEN:
                    case ENDFOR_TOKEN:
                    case ENDIF_TOKEN:
                    case ENDWHILE_TOKEN:
                    case NOT_TOKEN:
                    case OR_TOKEN:
                    case THEN_TOKEN:
                        throw new SystemException($"Token found out of context: {token}");
                    default:
                        value = GetGlobalOrLocal(grod, script, token, true);
                        if (value != null)
                        {
                            var userResult = Process(grod, value);
                            result.AddRange(userResult);
                            break;
                        }
                        throw new SystemException($"Unknown token: {token}");
                }
                return result;
            }
            var p = GetParameters(grod, script);
            if (p.Any(x => x.Type == MessageType.Error))
            {
                script.Index = script.Tokens.Length;
                result.AddRange(p.Where(x => x.Type == MessageType.Error));
                return result;
            }
            switch (token.ToLower())
            {
                case ABS_TOKEN:
                    Exec_Abs(grod, script, p, result);
                    break;
                case ADD_TOKEN:
                    Exec_Add(grod, script, p, result);
                    break;
                case ADDLIST_TOKEN:
                    Exec_AddList(grod, script, p, result);
                    break;
                case ADDTO_TOKEN:
                    Exec_AddTo(grod, script, p, result);
                    break;
                case BITWISEAND_TOKEN:
                    Exec_BitwiseAnd(grod, script, p, result);
                    break;
                case BITWISEOR_TOKEN:
                    Exec_BitwiseOr(grod, script, p, result);
                    break;
                case BITWISEXOR_TOKEN:
                    Exec_BitwiseXor(grod, script, p, result);
                    break;
                case CLEARARRAY_TOKEN:
                    Exec_ClearArray(grod, script, p, result);
                    break;
                case CLEARBIT_TOKEN:
                    Exec_ClearBit(grod, script, p, result);
                    break;
                case CLEARLIST_TOKEN:
                    Exec_ClearList(grod, script, p, result);
                    break;
                case COMMENT_TOKEN:
                    Exec_Comment(grod, script, p, result);
                    break;
                case CONCAT_TOKEN:
                    Exec_Concat(grod, script, p, result);
                    break;
                case CONTAINS_TOKEN:
                    Exec_Contains(grod, script, p, result);
                    break;
                case DATETIME_TOKEN:
                    Exec_DateTime(grod, script, p, result);
                    break;
                case DEBUG_TOKEN:
                    Exec_Debug(grod, script, p, result);
                    break;
                case DIV_TOKEN:
                    Exec_Div(grod, script, p, result);
                    break;
                case DIVTO_TOKEN:
                    Exec_DivTo(grod, script, p, result);
                    break;
                case EQ_TOKEN:
                    Exec_Equals(grod, script, p, result);
                    break;
                case EXEC_TOKEN:
                    Exec_Exec(grod, script, p, result);
                    break;
                case EXISTS_TOKEN:
                    Exec_Exists(grod, script, p, result);
                    break;
                case ISFALSE_TOKEN:
                    Exec_IsFalse(grod, script, p, result);
                    break;
                case FALSE_TOKEN:
                    Exec_IsFalse(grod, script, p, result);
                    break;
                case FLIPBIT_TOKEN:
                    Exec_Flipbit(grod, script, p, result);
                    break;
                case FOR_TOKEN:
                    Exec_For(grod, script, p, result);
                    break;
                case FOREACHKEY_TOKEN:
                    Exec_ForEachKey(grod, script, p, result);
                    break;
                case FOREACHLIST_TOKEN:
                    Exec_ForEachList(grod, script, p, result);
                    break;
                case FORMAT_TOKEN:
                    Exec_Format(grod, script, p, result);
                    break;
                case FROMBINARY_TOKEN:
                    Exec_FromBinary(grod, script, p, result);
                    break;
                case FROMHEX_TOKEN:
                    Exec_FromHex(grod, script, p, result);
                    break;
                case GE_TOKEN:
                    Exec_GreaterThanOrEquals(grod, script, p, result);
                    break;
                case GET_TOKEN:
                    Exec_Get(grod, script, p, result);
                    break;
                case GETARRAY_TOKEN:
                    Exec_GetArray(grod, script, p, result);
                    break;
                case GETBIT_TOKEN:
                    Exec_GetBit(grod, script, p, result);
                    break;
                case GETCHAR_TOKEN:
                    Exec_GetChar(grod, script, p, result);
                    break;
                case GETLIST_TOKEN:
                    Exec_GetList(grod, script, p, result);
                    break;
                case GETVALUE_TOKEN:
                    Exec_GetValue(grod, script, p, result);
                    break;
                case GOLABEL_TOKEN:
                    Exec_GoLabel(grod, script, p, result);
                    break;
                case GT_TOKEN:
                    Exec_GreaterThan(grod, script, p, result);
                    break;
                case INLIST_TOKEN:
                    Exec_ListContains(grod, script, p, result);
                    break;
                case INSERTATLIST_TOKEN:
                    Exec_InsertAtList(grod, script, p, result);
                    break;
                case ISBOOL_TOKEN:
                    Exec_IsBool(grod, script, p, result);
                    break;
                case ISNUMBER_TOKEN:
                    Exec_IsNumber(grod, script, p, result);
                    break;
                case ISSCRIPT_TOKEN:
                    Exec_IsScript(grod, script, p, result);
                    break;
                case LABEL_TOKEN:
                    Exec_Label(grod, script, p, result);
                    break;
                case LE_TOKEN:
                    Exec_LessThanOrEquals(grod, script, p, result);
                    break;
                case LEN_TOKEN:
                    Exec_Len(grod, script, p, result);
                    break;
                case LISTCONTAINS_TOKEN:
                    Exec_ListContains(grod, script, p, result);
                    break;
                case LISTCONTAINSALL_TOKEN:
                    Exec_ListContainsAll(grod, script, p, result);
                    break;
                case LISTLENGTH_TOKEN:
                    Exec_ListLength(grod, script, p, result);
                    break;
                case LOWER_TOKEN:
                    Exec_Lower(grod, script, p, result);
                    break;
                case LT_TOKEN:
                    Exec_LessThan(grod, script, p, result);
                    break;
                case MAX_TOKEN:
                    Exec_Max(grod, script, p, result);
                    break;
                case MIN_TOKEN:
                    Exec_Min(grod, script, p, result);
                    break;
                case MOD_TOKEN:
                    Exec_Mod(grod, script, p, result);
                    break;
                case MODTO_TOKEN:
                    Exec_ModTo(grod, script, p, result);
                    break;
                case MSG_TOKEN:
                    Exec_Msg(grod, script, p, result);
                    break;
                case MUL_TOKEN:
                    Exec_Mul(grod, script, p, result);
                    break;
                case MULTO_TOKEN:
                    Exec_MulTo(grod, script, p, result);
                    break;
                case NE_TOKEN:
                    Exec_NotEqual(grod, script, p, result);
                    break;
                case NEG_TOKEN:
                    Exec_Neg(grod, script, p, result);
                    break;
                case NEGTO_TOKEN:
                    Exec_NegTo(grod, script, p, result);
                    break;
                case ISNULL_TOKEN:
                    Exec_IsNull(grod, script, p, result);
                    break;
                case NULL_TOKEN:
                    Exec_IsNull(grod, script, p, result);
                    break;
                case ONGOLABEL_TOKEN:
                    Exec_OnGoLabel(grod, script, p, result);
                    break;
                case RAND_TOKEN:
                    Exec_Rand(grod, script, p, result);
                    break;
                case REMOVEATLIST_TOKEN:
                    Exec_RemoveAtList(grod, script, p, result);
                    break;
                case REPLACE_TOKEN:
                    Exec_Replace(grod, script, p, result);
                    break;
                case RND_TOKEN:
                    Exec_Rnd(grod, script, p, result);
                    break;
                case SCRIPT_TOKEN:
                    Exec_Script(grod, script, p, result);
                    break;
                case SET_TOKEN:
                    Exec_Set(grod, script, p, result);
                    break;
                case SETARRAY_TOKEN:
                    Exec_SetArray(grod, script, p, result);
                    break;
                case SETBIT_TOKEN:
                    Exec_SetBit(grod, script, p, result);
                    break;
                case SETCHAR_TOKEN:
                    Exec_SetChar(grod, script, p, result);
                    break;
                case SETEXTRA_TOKEN:
                    Exec_SetExtra(grod, script, p, result);
                    break;
                case SETLIST_TOKEN:
                    Exec_SetList(grod, script, p, result);
                    break;
                case SETOUTCHANNEL_TOKEN:
                    Exec_SetOutChannel(grod, script, p, result);
                    break;
                case SUB_TOKEN:
                    Exec_Sub(grod, script, p, result);
                    break;
                case SUBSTRING_TOKEN:
                    Exec_Substring(grod, script, p, result);
                    break;
                case SUBTO_TOKEN:
                    Exec_SubTo(grod, script, p, result);
                    break;
                case SWAP_TOKEN:
                    Exec_Swap(grod, script, p, result);
                    break;
                case TOBINARY_TOKEN:
                    Exec_ToBinary(grod, script, p, result);
                    break;
                case TOHEX_TOKEN:
                    Exec_ToHex(grod, script, p, result);
                    break;
                case TRIM_TOKEN:
                    Exec_Trim(grod, script, p, result);
                    break;
                case ISTRUE_TOKEN:
                    Exec_IsTrue(grod, script, p, result);
                    break;
                case TRUE_TOKEN:
                    Exec_IsTrue(grod, script, p, result);
                    break;
                case UPPER_TOKEN:
                    Exec_Upper(grod, script, p, result);
                    break;
                case WRITE_TOKEN:
                    Exec_Write(grod, script, p, result);
                    break;
                case WRITELINE_TOKEN:
                    Exec_WriteLine(grod, script, p, result);
                    break;
                default:
                    var userResult = GetUserDefinedFunctionValues(grod, script, token, p);
                    result.AddRange(userResult);
                    break;
            }
            return result;
        }
        catch (Exception ex)
        {
            // Handle exceptions and return error item
            var error = $"Error processing command at index {script.Index}: {ex.Message}";
            var extraInfo = new StringBuilder();
            for (int i = 0; i < script.Tokens.Length; i++)
            {
                var token = script.Tokens[i];
                if (token.Length > 50)
                {
                    token = string.Concat(token.AsSpan(0, 47), "...");
                }
                extraInfo.AppendLine($"{i}: {token}");
            }
            result.Add(new GrifMessage(MessageType.Error, error, extraInfo.ToString()));
            return result;
        }
    }
}
