using GrifLib;
using Newtonsoft.Json.Linq;
using static GrifLib.Common;
using static GrifLib.Dags;

/*
    [Test]
    public void Test_()
    {
        var key = "key";
        var value = "1";
        var expectedValue = "1";
        result = Process(grod, $"... {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"... {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }
*/

namespace Tests;

public class DagsTokenTests
{
    #region Setup ###DONE###
    private readonly Grod grod = new("test");
    private List<GrifMessage> result = [];

    [SetUp]
    public void Setup()
    {
        grod.Clear(true);
        result.Clear();
    }
    #endregion

    #region @abs ###DONE###

    [Test]
    public void Test_ABS_Positive()
    {
        var value = "23";
        var expectedValue = "23";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Negative()
    {
        var value = "-23";
        var expectedValue = "23";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Zero()
    {
        var value = "0";
        var expectedValue = "0";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_NoParams()
    {
        result = Process(grod, $"{ABS_TOKEN})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ABS_Empty()
    {
        var value = "\"\"";
        var expectedValue = "0";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ABS_Null()
    {
        string? value = null;
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ABS_Invalid()
    {
        var value = "abc";
        result = Process(grod, $"{ABS_TOKEN}{value})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @addlist ###DONE###

    [Test]
    public void Test_ADDLIST_OneItem()
    {
        var key = "key";
        var value = "1";
        var expectedValue = "1";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_TwoItems()
    {
        var key = "key";
        var value1 = "1";
        var expectedValue1 = "1";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        var value2 = "2";
        var expectedValue2 = $"{value1},{value2}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    [Test]
    public void Test_ADDLIST_Empty()
    {
        var key = "key";
        var value = "";
        var expectedValue = "";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_ValueEmptyValue()
    {
        var key = "key";
        var value1 = "value1";
        var value2 = "";
        var value3 = "value3";
        var expectedValue = $"{value1},,{value3}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_ValueNullValue()
    {
        var key = "key";
        var value1 = "value1";
        string? value2 = null;
        var value3 = "value3";
        var expectedValue = $"{value1},,{value3}";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDLIST_NullNullNull()
    {
        var key = "key";
        string? value1 = null;
        string? value2 = null;
        string? value3 = null;
        var expectedValue = $",,";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {ADDLIST_TOKEN}{key},{value3}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @addto ###DONE###

    [Test]
    public void Test_ADDTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "9";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADDTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADDTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADDTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {ADDTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @add ###DONE###

    [Test]
    public void Test_ADD_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "9";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADD_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{ADD_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_ADD_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_ADD_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{ADD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @and ###DONE###

    [Test]
    public void Test_AND_TrueAndTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AND_TrueAndFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AND_FalseAndTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {AND_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_AND_FalseAndFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {AND_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @bitwiseand ###DONE###

    [Test]
    public void Test_BitwiseAnd()
    {
        var expectedValue1 = "2";
        result = Process(grod, $"{BITWISEAND_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "0";
        result = Process(grod, $"{BITWISEAND_TOKEN}8,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    #endregion

    #region @bitwiseor ###DONE###

    [Test]
    public void Test_BitwiseOr()
    {
        var expectedValue1 = "7";
        result = Process(grod, $"{BITWISEOR_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "10";
        result = Process(grod, $"{BITWISEOR_TOKEN}8,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    #endregion

    #region @bitwisexor ###DONE###

    [Test]
    public void Test_BitwiseXor()
    {
        var expectedValue1 = "5";
        result = Process(grod, $"{BITWISEXOR_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "15";
        result = Process(grod, $"{BITWISEXOR_TOKEN}8,7)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
    }

    #endregion

    #region @cleararray ###DONE###

    [Test]
    public void Test_ClearArray()
    {
        var key = "abc";
        var value = "123";
        var expectedValue = "";
        result = Process(grod, $"{SETARRAY_TOKEN}{key},2,3,{value}) {CLEARARRAY_TOKEN}{key}) {GETARRAY_TOKEN}{key},2,3)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @clearbit ###DONE###

    [Test]
    public void Test_ClearBit()
    {
        var expectedValue1 = "3";
        result = Process(grod, $"{CLEARBIT_TOKEN}7,2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));

        var expectedValue2 = "6";
        result = Process(grod, $"{CLEARBIT_TOKEN}7,0)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));

        var expectedValue3 = "0";
        result = Process(grod, $"{CLEARBIT_TOKEN}1073741824,30)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue3));
    }

    #endregion

    #region @clearlist ###DONE###

    [Test]
    public void Test_ClearList()
    {
        var key = "abc";
        var value1 = "123";
        var value2 = "456";
        var expectedValue = "";
        result = Process(grod, $"{ADDLIST_TOKEN}{key},{value1}) {ADDLIST_TOKEN}{key},{value2}) {CLEARLIST_TOKEN}{key}) {GETLIST_TOKEN}{key},0)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @comment ###DONE###

    [Test]
    public void Test_COMMENT()
    {
        var script = $"{COMMENT_TOKEN}\"this is a comment\")";
        result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void Test_COMMENT_TwoComments()
    {
        string script = $"{COMMENT_TOKEN}\"This is a comment\n\") {COMMENT_TOKEN}\"Another comment\")";
        result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    #endregion

    #region @concat ###DONE###

    [Test]
    public void Test_Concat()
    {
        var expectedValue = "abcdef123";
        result = Process(grod, $"{CONCAT_TOKEN}abc, def, 123)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @containslist
    #endregion

    #region @contains
    #endregion

    #region @datetime ###DONE###

    [Test]
    public void Test_DATETIME()
    {
        var script = @"@write(@datetime(""MM-dd-yyyy HH:mm:ss""))";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(19)); // value will vary, length fixed
        Assert.That(result[0].ExtraValue, Has.Length.EqualTo(6)); // value will vary, length fixed
    }

    [Test]
    public void Test_DATETIME_UTC()
    {
        var script = @"@write(@datetime(""MM-dd-yyyy HH:mm:ss"",utc))";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(19)); // value will vary, length fixed
        Assert.That(result[0].ExtraValue, Is.EqualTo("UTC"));
    }

    [Test]
    public void Test_DATETIME_Roundtrip()
    {
        var script = @"@write(@datetime(""O""))";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(33)); // value will vary, length fixed
        Assert.That(result[0].ExtraValue, Has.Length.EqualTo(6)); // value will vary, length fixed
    }

    [Test]
    public void Test_DATETIME_RoundtripUTC()
    {
        var script = @"@write(@datetime(""O"",utc))";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(MessageType.Text));
        Assert.That(result[0].Value, Has.Length.EqualTo(28)); // value will vary, length fixed
        Assert.That(result[0].Value[^1], Is.EqualTo('Z'));
        Assert.That(result[0].ExtraValue, Is.EqualTo("UTC"));
    }

    #endregion

    #region @debug ###DONE###

    [Test]
    public void Test_Debug()
    {
        var expectedValue1 = "### this is a debug comment";
        Process(grod, $"{SET_TOKEN}{DEBUG_FLAG},true)");
        result = Process(grod, $"{DEBUG_TOKEN}\"### this is a debug comment\")");
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));

        var expectedValue2 = "579";
        Process(grod, $"{SET_TOKEN}{DEBUG_FLAG},true)");
        result = Process(grod, $"{DEBUG_TOKEN}{ADD_TOKEN}123,456))");
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue2));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));

        Process(grod, $"{SET_TOKEN}{DEBUG_FLAG},false)");
        result = Process(grod, $"{DEBUG_TOKEN}\"### this is a comment\")");
        Assert.That(result, Has.Count.EqualTo(0));
    }

    #endregion

    #region @divto ###DONE###

    [Test]
    public void Test_DIVTO_Value()
    {
        var key = "key";
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Remainder()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIVTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIVTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {DIVTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @div ###DONE###

    [Test]
    public void Test_DIV_Value()
    {
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "5";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Remainder()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Blank()
    {
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{DIV_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_NotExists()
    {
        var notexists = "notexists";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{DIV_TOKEN}{GET_TOKEN}{notexists}),{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_DIV_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_DIV_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{DIV_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @do
    #endregion

    #region @elseif
    #endregion

    #region @else
    #endregion

    #region @endforeachkey
    #endregion

    #region @endforeachlist
    #endregion

    #region @endfor
    #endregion

    #region @endif
    #endregion

    #region @endwhile
    #endregion

    #region @eq
    #endregion

    #region @exec
    #endregion

    #region @exists
    #endregion

    #region @false
    #endregion

    #region @flipbit
    #endregion

    #region @foreachkey
    #endregion

    #region @foreachlist
    #endregion

    #region @format
    #endregion

    #region @for
    #endregion

    #region @frombinary
    #endregion

    #region @fromhex
    #endregion

    #region @getarray
    #endregion

    #region @getbit
    #endregion

    #region @getchar
    #endregion

    #region @getinchannel
    #endregion

    #region @getlist
    #endregion

    #region @getvalue
    #endregion

    #region @get
    #endregion

    #region @ge
    #endregion

    #region @golabel
    #endregion

    #region @gt
    #endregion

    #region @if
    #endregion

    #region @inlist
    #endregion

    #region @insertatlist
    #endregion

    #region @isbool
    #endregion

    #region @isfalse
    #endregion

    #region @isnull
    #endregion

    #region @isnumber
    #endregion

    #region @isscript
    #endregion

    #region @istrue
    #endregion

    #region @label
    #endregion

    #region @len
    #endregion

    #region @le
    #endregion

    #region @listlength
    #endregion

    #region @lower
    #endregion

    #region @lt
    #endregion

    #region @max
    #endregion

    #region @min
    #endregion

    #region @modto ###DONE###

    [Test]
    public void Test_MODTO_Value()
    {
        var key = "key";
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Remainder()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_ByZero()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_Negative1()
    {
        // -20 % 30 = -20
        var key = "key";
        var value1 = "-20";
        var value2 = "30";
        var expectedValue = "-20";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Negative2()
    {
        // -20 % 6 = -2
        var key = "key";
        var value1 = "-20";
        var value2 = "6";
        var expectedValue = "-2";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Negative3()
    {
        // 20 % -6 = 2
        var key = "key";
        var value1 = "20";
        var value2 = "-6";
        var expectedValue = "2";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Negative4()
    {
        // -20 % -6 = -2
        var key = "key";
        var value1 = "-20";
        var value2 = "-6";
        var expectedValue = "-2";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MODTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MODTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MODTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @mod ###DONE###

    [Test]
    public void Test_MOD_Value()
    {
        var value1 = "20";
        var value2 = "4";
        var expectedValue = "0";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Remainder()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_ByZero()
    {
        var value1 = "5";
        var value2 = "0";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_Negative1()
    {
        // -20 % 30 = -20
        var value1 = "-20";
        var value2 = "30";
        var expectedValue = "-20";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative2()
    {
        // -20 % 6 = -2
        var value1 = "-20";
        var value2 = "6";
        var expectedValue = "-2";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative3()
    {
        // 20 % -6 = 2
        var value1 = "20";
        var value2 = "-6";
        var expectedValue = "2";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Negative4()
    {
        // -20 % -6 = -2
        var value1 = "-20";
        var value2 = "-6";
        var expectedValue = "-2";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Blank1()
    {
        // 0 % 5 = 0
        var value1 = "";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Blank2()
    {
        var value1 = "5";
        var value2 = "";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        result = Process(grod, $"{MOD_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_NotExists()
    {
        // 0 % 5 = 0
        var notexists = "notexists";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MOD_TOKEN}{GET_TOKEN}{notexists}),{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MOD_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MOD_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{MOD_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @msg ###DONE###

    [Test]
    public void Test_MSG()
    {
        var key = "key";
        var value = "Hello, World!";
        var expectedValue = "Hello, World!";
        grod.Set(key, value);
        string script = $"{MSG_TOKEN}{key})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));
    }

    #endregion

    #region @multo ###DONE###

    [Test]
    public void Test_MULTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "20";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MULTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MULTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MULTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {MULTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @mul ###DONE###

    [Test]
    public void Test_MUL_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "20";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_ValueNegative()
    {
        var value1 = "5";
        var value2 = "-4";
        var expectedValue = "-20";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "0";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_FromBlank()
    {
        var value1 = "";
        var value2 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MUL_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "0";
        result = Process(grod, $"{MUL_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_MUL_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_MUL_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{MUL_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @negto ###DONE###

    [Test]
    public void Test_NEGTO()
    {
        var key = "key";
        var value = "5";
        var expectedValue = "-5";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEGTO_Negative()
    {
        var key = "key";
        var value = "-5";
        var expectedValue = "5";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEGTO_Zero()
    {
        var key = "key";
        var value = "0";
        var expectedValue = "0";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEGTO_Error()
    {
        var key = "key";
        var value = "abc";
        grod.Set(key, value);
        string script = $"{NEGTO_TOKEN}{key}) {GET_TOKEN}{key})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @neg ###DONE###

    [Test]
    public void Test_NEG()
    {
        var value = "5";
        var expectedValue = "-5";
        string script = $"{NEG_TOKEN}{value})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEG_Negative()
    {
        var value = "-5";
        var expectedValue = "5";
        string script = $"{NEG_TOKEN}{value})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEG_Zero()
    {
        var value = "0";
        var expectedValue = "0";
        string script = $"{NEG_TOKEN}{value})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NEG_Error()
    {
        var value = "abc";
        string script = $"{NEG_TOKEN}{value})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @ne
    #endregion

    #region @nl
    #endregion

    #region @not

    [Test]
    public void Test_NOT_AND_TrueAndNotTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {NOT_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NOT_AND_TrueAndNotFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {AND_TOKEN} {NOT_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NOT_OR_FalseOrNotFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {NOT_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_NOT_OR_FalseOrNotTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {NOT_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @null
    #endregion

    #region @ongolabel
    #endregion

    #region @or

    [Test]
    public void Test_OR_FalseOrFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition not met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_OR_TrueOrFalse()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {OR_TOKEN} {FALSE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_OR_FalseOrTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {FALSE} {OR_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_OR_TrueOrTrue()
    {
        var valueMet = "Condition met";
        var valueNotMet = "Condition not met";
        var expectedValue = "Condition met";
        string script = $"{IF_TOKEN} {TRUE} {OR_TOKEN} {TRUE} {THEN_TOKEN} {WRITE_TOKEN}\"{valueMet}\") {ELSE_TOKEN}  {WRITE_TOKEN}\"{valueNotMet}\") {ENDIF_TOKEN}";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @rand
    #endregion

    #region @removeatlist ###DONE###

    [Test]
    public void Test_RemoveAtList()
    {
        var key = "key";
        var value = "123";
        var expectedValue = "123";
        Process(grod, $"{SETLIST_TOKEN}{key},3,{value})");
        Process(grod, $"{REMOVEATLIST_TOKEN}{key},0)");
        result = Process(grod, $"{GETLIST_TOKEN}{key},2)");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion

    #region @replace
    #endregion

    #region @return
    #endregion

    #region @rnd
    #endregion

    #region @script
    #endregion

    #region @setarray
    #endregion

    #region @setbit
    #endregion

    #region @setchar
    #endregion

    #region @setextra
    #endregion

    #region @setlist
    #endregion

    #region @setoutchannel
    #endregion

    #region @set
    #endregion

    #region @substring
    #endregion

    #region @subto ###DONE###

    [Test]
    public void Test_SUBTO_Value()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Blank()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Null()
    {
        var key = "key";
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUBTO_GetNull()
    {
        var key = "key";
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{GET_TOKEN}{notExists})) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_NotExists()
    {
        var key = "key";
        var value2 = "5";
        var expectedValue = "-5";
        result = Process(grod, $"{SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUBTO_Invalid1()
    {
        var key = "key";
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUBTO_Invalid2()
    {
        var key = "key";
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SET_TOKEN}{key},{value1}) {SUBTO_TOKEN}{key},{value2}) {GET_TOKEN}{key})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @sub ###DONE###

    [Test]
    public void Test_SUB_Value()
    {
        var value1 = "5";
        var value2 = "4";
        var expectedValue = "1";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_ValueNegative()
    {
        var value1 = "5";
        var value2 = "14";
        var expectedValue = "-9";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Blank()
    {
        var value1 = "5";
        var value2 = "";
        var expectedValue = "5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_FromBlank()
    {
        var value1 = "";
        var value2 = "5";
        var expectedValue = "-5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Null()
    {
        var value1 = "5";
        var value2 = NULL;
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUB_GetNull()
    {
        var notExists = "notexists";
        var value1 = "5";
        var expectedValue = "5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{GET_TOKEN}{notExists}))");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Test_SUB_Invalid1()
    {
        var value1 = "abc";
        var value2 = "5";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    [Test]
    public void Test_SUB_Invalid2()
    {
        var value1 = "5";
        var value2 = "abc";
        result = Process(grod, $"{SUB_TOKEN}{value1},{value2})");
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.True);
    }

    #endregion

    #region @swap
    #endregion

    #region @then
    #endregion

    #region @tobinary
    #endregion

    #region @tohex
    #endregion

    #region @trim
    #endregion

    #region @true
    #endregion

    #region @upper
    #endregion

    #region @while ###DONE###

    [Test]
    public void Test_WHILE()
    {
        var script = @"
            @set(_a,0)
            @while @lt(@get(_a),3) @do
                @addto(_a,1)
                @write(@get(_a))
            @endwhile
            @write(""xyz"")
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, "1"),
            new(MessageType.Text, "2"),
            new(MessageType.Text, "3"),
            new(MessageType.Text, "xyz")
        };
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result, Is.EqualTo(answer));
    }

    [Test]
    public void Test_WHILE_Return()
    {
        var script = @"
            @set(_a,0)
            @while @lt(@get(_a),3) @do
                @addto(_a,1)
                @write(@get(_a))
                @if @eq(@get(_a),2) @then
                    @return
                @endif
            @endwhile
            @write(""xyz"")
            ";
        var answer = new List<GrifMessage> {
            new(MessageType.Text, "1"),
            new(MessageType.Text, "2")
        };
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result, Is.EqualTo(answer));
    }

    #endregion

    #region @writeline ###DONE###

    [Test]
    public void Test_WRITELINE()
    {
        var value1 = "abc";
        var expectedValue1 = "abc";
        string script = $"{WRITELINE_TOKEN}{value1})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));
    }

    [Test]
    public void Test_WRITELINE_Concatenate()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue1 = "abc";
        var expectedValue2 = "def";
        string script = $"{WRITELINE_TOKEN}{value1},{value2})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(expectedValue2));
        Assert.That(result[2].Value, Is.EqualTo(NL_CHAR));
    }

    [Test]
    public void Test_WRITELINE_FromGet()
    {
        var key = "key";
        var value = "abc";
        var expectedValue = "abc";
        grod.Set(key, value);
        string script = $"{WRITELINE_TOKEN}{GET_TOKEN}{key}))";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
        Assert.That(result[1].Value, Is.EqualTo(NL_CHAR));
    }

    #endregion

    #region @write ###DONE###

    [Test]
    public void Test_WRITE()
    {
        var value1 = "abc";
        var expectedValue1 = "abc";
        string script = $"{WRITE_TOKEN}{value1})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
    }

    [Test]
    public void Test_WRITE_Concatenate()
    {
        var value1 = "abc";
        var value2 = "def";
        var expectedValue1 = "abc";
        var expectedValue2 = "def";
        string script = $"{WRITE_TOKEN}{value1},{value2})";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue1));
        Assert.That(result[1].Value, Is.EqualTo(expectedValue2));
    }

    [Test]
    public void Test_WRITE_FromGet()
    {
        var key = "key";
        var value = "abc";
        var expectedValue = "abc";
        grod.Set(key, value);
        string script = $"{WRITE_TOKEN}{GET_TOKEN}{key}))";
        var result = Process(grod, script);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.Any(x => x.Type == MessageType.Error), Is.False);
        Assert.That(result[0].Value, Is.EqualTo(expectedValue));
    }

    #endregion
}
